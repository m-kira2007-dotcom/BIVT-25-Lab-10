namespace Lab9.White
{
    public abstract class White
    {
        protected string _input;
        
        public string Input=>_input;



        protected White(string input)
        {
            _input=input;
        }

        public abstract void Review();

        public void ChangeText(string text)
        {
            _input = text;
            Review();
        }
    }
}