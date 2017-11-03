using System;
using RosalieSegerdorf_MVC1.Data.Models;

namespace RosalieSegerdorf_MVC1.Helpers
{
    public static class EnumToStringHelper
    {
        public static string EnumToString(this Expertise valueOfExpertise)
        {
            switch (valueOfExpertise)
            {
                case Expertise.Designer:
                    return "Designer";
                case Expertise.Advertise:
                    return "Advertise";
                case Expertise.Analyzer:
                    return "Analyzer";
                case Expertise.Developer:
                    return "Developer";
                case Expertise.Engineering:
                    return "Engineering";
                case Expertise.Management:
                    return "Management";
                case Expertise.Sales:
                    return "Sales";
                case Expertise.ScrumMaster:
                    return "Scrum Master";
                case Expertise.Support:
                    return "Support";
                case Expertise.Tester:
                    return "Tester";
                default: throw new ArgumentOutOfRangeException();

            }
        }

    }
}