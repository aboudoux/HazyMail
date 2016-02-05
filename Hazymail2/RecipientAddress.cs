namespace Hazymail
{
    public class RecipientAddress : IRecipientAddress
    {
        private readonly string _name;
        private readonly string _email;

        public RecipientAddress(string line)
        {            
            if (line.Contains("<")) {
                var startIndex = line.IndexOf("<", System.StringComparison.Ordinal);
                _name = line.Substring(0, startIndex).Trim();
                _email = line.Substring(startIndex + 1).Trim('<', '>', ' ');
            }
            else 
                _email = line;            
        }

        public string Name
        {
            get { return _name; }            
        }

        public string Email
        {
            get { return _email; }            
        }
    }
}