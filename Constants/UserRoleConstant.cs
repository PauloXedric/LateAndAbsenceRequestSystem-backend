namespace DLARS.Constants
{
    public static class UserRoleConstant
    {
        public const string Secretary = "Secretary";
        public const string Chairperson = "Chairperson";
        public const string Director = "Director";

        public const string SecretaryAndChairperson = Secretary + "," + Chairperson;
        public const string SecretaryAndDirector = Secretary + "," + Director;
        public const string ChairpersonAndDirector = Chairperson + "," + Director;

        public const string AllAdminRoles = Secretary + "," + Chairperson + "," + Director;     
    }
}
