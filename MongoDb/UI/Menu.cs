using AutoMapper;
using Microsoft.Extensions.Configuration;
using SocialNetwork.Models;

namespace SocialNetwork.UI
{
    internal class Menu
    {
        string _userId;
        string _userFullName;
        SocialNetworkRepository _socialNetworkRepository;

        public Menu()
        {
            IMapper mapper = SetupMapper();
            string _connString = new ConfigurationBuilder().AddJsonFile("C:\\Users\\PC\\Desktop\\C#\\SocialNetwork\\SocialNetwork\\appsettings.json").Build().GetConnectionString("SN");
            _socialNetworkRepository = new SocialNetworkRepository(_connString, "SocialNetwork", mapper);
        }

        IMapper SetupMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = config.CreateMapper();
            return mapper;
        }


        public void ShowMenu()
        {
            Console.WriteLine("\t\t\t\tWelcome to our social network!");

            Console.WriteLine("Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Password:");
            string password = Console.ReadLine();

            bool isLoggedIn = _socialNetworkRepository.Login(email, password);
            //bool isLoggedIn = _socialNetworkRepository.Login("johndoe@example.com", "real_password_1");

            if (isLoggedIn)
            {
                User user = _socialNetworkRepository.CurrentUser;
                _userId = user.Id;
                Console.WriteLine($"\nWelcome {user.FirstName} {user.LastName}\n");

                ShowUserPosts(_userId);


                bool isMenu1 = true;

                while (isMenu1)
                {
                    Console.WriteLine("Find user or exit:");
                    int opt = int.Parse(Console.ReadLine());

                    switch (opt)
                    {

                        case 1:
                            {

                                Console.WriteLine("\nEnter name of user which you want to find:");
                                string name = Console.ReadLine();

                                Console.WriteLine("Enter surname of user which you want to find:");
                                string surname = Console.ReadLine();

                                _userFullName = name + " " + surname;

                                ShowTextMenu();

                                bool isMenu = true;

                                while (isMenu)
                                {
                                    Console.WriteLine("\nEnter option:");
                                    int option = int.Parse(Console.ReadLine());

                                    switch (option)
                                    {
                                        case 1: { AddUserToFriend(name, surname); break; }
                                        case 2: { DeleteUserFromFriend(name, surname); break; }
                                        case 3: { ShowUserPosts(name, surname); break; }
                                        case 4: { AddCommentToPost(); break; }
                                        case 5: { AddLikeToPost(); break; }
                                        case 6: { DeleteLikeFromPost(); break; }
                                        case 0: { isMenu = false; break; }
                                    }
                                }

                                break;
                            }

                        case 0:
                            {
                                Console.WriteLine("Bye!");
                                isMenu1 = false;
                                break;
                            }
                    }
                }
            }

            else
            {
                Console.WriteLine("You enter invalid email and password");
            }

        }


        void ShowTextMenu()
        {
            Console.WriteLine("\n1 - Add this user to friends");
            Console.WriteLine("2 - Delete this user from friends");
            Console.WriteLine("3 - Show posts of this user");
            Console.WriteLine("4 - Add comment to post");
            Console.WriteLine("5 - Add like to post");
            Console.WriteLine("6 - Delete like from post");
            Console.WriteLine("0 - Return to start menu");
        }




        void ShowUserPosts(string id)
        {
            List<Post> posts = _socialNetworkRepository.DisplayFriendPosts(id);

            foreach (Post post in posts)
            {
                Console.WriteLine($"\t{post.User}\n{post.Content}");
            }
        }

        void AddUserToFriend(string name, string surname)
        {
            _socialNetworkRepository.AddFriend(name, surname);
        }

        void DeleteUserFromFriend(string name, string surname)
        {
            _socialNetworkRepository.DeleteFriend(name, surname);
        }

        void ShowUserPosts(string name, string surname)
        {
            var posts = _socialNetworkRepository.GetUserPosts(name, surname);

            Console.WriteLine($"Posts of {name} {surname}:");

            foreach (Post post in posts)
            {
                Console.WriteLine($"\t{post.Title}");
                Console.WriteLine($"\t\t{post.Content}");
            }
        }

        void AddCommentToPost()
        {
            Console.WriteLine("\nTitle of post which you want to comment:");
            string title = Console.ReadLine();

            Console.WriteLine("Comment:");
            string comment = Console.ReadLine();


            _socialNetworkRepository.AddCommentToPost(title, comment, _userFullName, DateTime.Now);
        }

        void AddLikeToPost()
        {
            Console.WriteLine("\nTitle of post which you want to like:");
            string title = Console.ReadLine();

            _socialNetworkRepository.AddLikeToPost(title, _userFullName);
        }

        void DeleteLikeFromPost()
        {
            Console.WriteLine("\nTitle of post in which you want to remove like:");
            string title = Console.ReadLine();

            _socialNetworkRepository.DeleteLikeFromPost(title, _userFullName);
        }
    }
}