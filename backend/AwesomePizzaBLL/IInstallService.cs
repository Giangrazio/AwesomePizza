using GenericUnitOfWork.UoW;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwesomePizzaDAL.Entities;
using AwesomePizzaDAL.Repositories;
using MagicCrypto;
using AwesomePizzaBLL.Structure;

namespace AwesomePizzaBLL
{
    public interface IInstallService
    {
        void Initialize();
    }

    public class InstallService : IInstallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICrypto _cryptor;
        private readonly string _secretSeed;

        public InstallService(IUnitOfWork unitOfWork, ICrypto cryptor, ITokenCryptoSettings criptosettings)
        {
            _unitOfWork = unitOfWork;
            _cryptor = cryptor;
            _secretSeed = criptosettings.PwdAccountSeedKey;
        }

        public void Initialize()
        {
            InitializeUser();
            InitializeProduct();    
        }

        private void InitializeUser()
        {
            var repo = _unitOfWork.GenericRepository<UserRepository>();
            var usersExist = repo.GetAll();
            var defaultUsers = GetDefaultUsers();
            if (!usersExist.Any())
            {
                repo.AddRange(defaultUsers);
                _unitOfWork.SaveChanges();
            }
            else
            {
                defaultUsers.ForEach(n =>
                {
                    var user = usersExist.FirstOrDefault(u => u.CodeValue.ToLower() == n.CodeValue.ToLower());
                    if (user != null)
                    {
                        user.Password = n.Password;
                        user.NickName = n.NickName;
                        user.Denomination = n.Denomination;
                        user.RoleName = n.RoleName;
                        repo.Update(user);
                    }
                    else
                    {
                        repo.Add(n);
                    }
                });

                _unitOfWork.SaveChanges();
            }
        }


        private void InitializeProduct()
        {
            var repo = _unitOfWork.GenericRepository<ProductRepository>();
            var products = repo.GetAll();
            var defaultProducts = GetDefaultProducts();
            if (!products.Any())
            {
                repo.AddRange(defaultProducts);
                _unitOfWork.SaveChanges();
            }
            else
            {
                defaultProducts.ForEach(n =>
                {
                    var u = products.FirstOrDefault(u => u.CodeValue.ToLower() == n.CodeValue.ToLower());
                    if (u != null)
                    {
                        u.Name = n.Name;
                        u.Price = n.Price;
                        u.Description = n.Description;
                        u.ImageUrl = n.ImageUrl;
                        u.Category = n.Category;
                        repo.Update(u);
                    }
                    else
                    {
                        repo.Add(n);
                    }
                });

                _unitOfWork.SaveChanges();
            }
        }


        private List<UserEntity> GetDefaultUsers()
        {
            List<UserEntity> users = new List<UserEntity>
            {
                new UserEntity()
                {
                    CodeValue = "U-01",
                    Denomination = "Pizzaiolo Uno",
                    NickName = "pizzaiolo1",
                    Password = _cryptor.EncryptStringAES("pizzaiolo1", _secretSeed),
                    RoleName = "Operatore"
                },
                new UserEntity()
                {
                    CodeValue = "U-02",
                    Denomination = "Pizzaiolo Due",
                    NickName = "pizzaiolo2",
                    Password = _cryptor.EncryptStringAES("pizzaiolo2", _secretSeed),
                    RoleName = "Operatore"
                },
            };
            return users;
        }

        private List<ProductEntity> GetDefaultProducts()
        {
            List<ProductEntity> products = new List<ProductEntity>()
            {
                 new ProductEntity
            {
                Name = "Margherita",
                CodeValue = "P-001",
                Type = TypeOfProduct.Pizza,
                Category = "Classic",
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 7.50m,
                Description = "Classic Margherita with fresh mozzarella and basil."
            },
            new ProductEntity
            {
                Name = "Pepperoni",
                CodeValue = "P-002",
                Category = "Meat",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 8.50m,
                Description = "Spicy pepperoni with melted mozzarella."
            },
            new ProductEntity
            {
                Name = "Hawaiian",
                CodeValue = "P-003",
                Category = "Exotic",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 9.00m,
                Description = "Sweet and savory with pineapple and ham."
            },
            new ProductEntity
            {
                Name = "Four Cheese",
                CodeValue = "P-004",
                Category = "Vegetarian",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 8.00m,
                Description = "A rich blend of mozzarella, cheddar, gouda, and parmesan."
            },
            new ProductEntity
            {
                Name = "BBQ Chicken",
                CodeValue = "P-005",
                Category = "Specialty",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 9.50m,
                Description = "Grilled chicken smothered in BBQ sauce with red onions."
            },
            new ProductEntity
            {
                Name = "Veggie Delight",
                CodeValue = "P-006",
                Category = "Vegetarian",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 8.75m,
                Description = "Fresh vegetables like bell peppers, olives, and mushrooms."
            },
            new ProductEntity
            {
                Name = "Meat Lover's",
                CodeValue = "P-007",
                Category = "Meat",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 10.00m,
                Description = "Loaded with bacon, sausage, and pepperoni."
            },
            new ProductEntity
            {
                Name = "Seafood Special",
                CodeValue = "P-008",
                Category = "Specialty",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 11.00m,
                Description = "A medley of shrimp, calamari, and scallops."
            },
            new ProductEntity
            {
                Name = "Mushroom & Truffle",
                CodeValue = "P-009",
                Category = "Gourmet",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 12.50m,
                Description = "Gourmet mushrooms with a hint of truffle oil."
            },
            new ProductEntity
            {
                Name = "Spicy Veggie",
                CodeValue = "P-010",
                Category = "Vegetarian",
                Type = TypeOfProduct.Pizza,
                ImageUrl = "https://media-assets.lacucinaitaliana.it/photos/63c0401ffb4d383e74f344dd/16:9/w_1920,c_limit/migliore%20pizza%20milano%20pizzeria.jpg",
                Price = 9.25m,
                Description = "Spicy jalapeños with a mix of fresh vegetables."
            }
            };

            return products;
        }
    }
}
