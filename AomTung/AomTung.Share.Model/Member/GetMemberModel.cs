namespace AomTung.Share.Model.Member
{
    public class GetMemberModel
    {
        public int id { get; set; }
        public string username { get; set; } = null!;
        public string firstname { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public DateTime? date_of_birth { get; set; }
    }
}
