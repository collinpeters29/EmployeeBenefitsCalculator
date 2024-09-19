namespace Api.Helpers
{
    public static class DataHelper
    {

        //Returns True if age is GREATER THAN Age Cutoff
        public static bool isOverAgeCutoff(DateTime dateOfBirth, int AgeCutoff)
        {
            var age = GetAge(dateOfBirth);
            if (age >= AgeCutoff) return true;
            else return false;
        }

        private static int GetAge(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;

            //If their birthday has not come yet this year subtract 1 from age
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            return age;
        }

    }
}
