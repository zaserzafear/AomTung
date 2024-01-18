namespace AomTung.DataAccessLayer.Extensions
{
    public interface IMySqlHelper
    {
        string aes_encrypt(string input, string saltKey);
        string aes_decrypt(string input, string saltKey);
    }
}
