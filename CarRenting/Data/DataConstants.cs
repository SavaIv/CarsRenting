namespace CarRenting.Data
{
    public class DataConstants
    {
        public class Car
        {
            public const int CarBrandMaxLength = 20;
            public const int CarBrandMinLength = 2;
            public const int CarModelMaxLenght = 30;
            public const int CarModelMinLenght = 2;
            public const int CarDescriptionMaxlength = 1000;
            public const int CarDescriptionMinlength = 10;
            public const int CarYearMinValue = 2000;
            public const int CarYearMaxValue = 2100;
        }

        public class Category
        {
            public const int CategoryNameMaxLength = 25;
        }

        public class Dealer
        {
            public const int DealerNameMinLength = 2;
            public const int DealerNameMaxLength = 30;
            public const int DealerPhoneMinLength = 6;
            public const int DealerPhoneMaxLength = 30;
        }


    }
}
