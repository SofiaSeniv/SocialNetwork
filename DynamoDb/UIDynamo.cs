using DAL;

namespace SocialNetwork.UI
{
    public class UIDynamo
    {
        private readonly SocialNetworkRepositoryDynamo _repository;

        public UIDynamo(SocialNetworkRepositoryDynamo repository)
        {
            _repository = repository;
        }

        public async Task Run()
        {
            await _repository.CreateSocialMediaTable(); 

            while (true)
            {
                PrintMenu();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EditPost();
                        break;

                    case "2":
                        EditComment();
                        break;

                    case "3":
                        GetSortedCommentsForPost();
                        break;

                    case "4":
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("1. Edit Post");
            Console.WriteLine("2. Edit Comment");
            Console.WriteLine("3. Get Sorted Comments for Post");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");
        }

        private void EditPost()
        {
            Console.Write("Enter the Post ID: ");
            string postId = Console.ReadLine();
            Console.Write("Enter the new content for the Post: ");
            string newPostContent = Console.ReadLine();
            _repository.EditPost(postId, newPostContent).Wait();
            Console.WriteLine("Post successfully edited!");
        }

        private void EditComment()
        {
            Console.Write("Enter the Post ID: ");
            string commentPostId = Console.ReadLine();
            Console.Write("Enter the Comment ID: ");
            string commentId = Console.ReadLine();
            Console.Write("Enter the new Comment text: ");
            string newCommentText = Console.ReadLine();
            _repository.EditComment(commentPostId, commentId, newCommentText).Wait();
            Console.WriteLine("Comment successfully edited!");
        }

        private async void GetSortedCommentsForPost()
        {
            Console.Write("Enter the Post ID: ");
            string postWithCommentsId = Console.ReadLine();
            var sortedComments = await _repository.GetSortedCommentsForPost(postWithCommentsId);

            foreach (var comment in sortedComments)
            {
                Console.WriteLine($"CommentId: {comment["CommentId"].N}, CommentText: {comment["CommentText"].S}, ModifiedDateTime: {comment["ModifiedDateTime"].S}");
            }
        }
    }
}