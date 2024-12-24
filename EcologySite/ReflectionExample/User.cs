namespace ReflectionExample
{
    public class User
    {
        private int _age = 16;

        public int Id { get; set; }
        public string Name { get; set; }

        public void DoCool(int x, int cost)
        {

        }

        public void SayYouAge()
        {
            Console.WriteLine(_age);
        }
    }
}
