using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace TestApp.Entity
{
    [DataContract]
    public class ProductEntity : EntityObject
    {
        private int productId;
        [DataMember(Name = "ProductId")]
        public int ProductId
        {
            get
            {
                return productId;
            }
            set
            {
                productId = value;
                Id = productId;
            }
        }

        [DataMember(Name = "Number")]
        public int Number { get; set; }

        [DataMember(Name = "ProductType")]
        public string ProductType { get; set; }

        [DataMember(Name = "ReleaseDate")]
        public DateTime ReleaseDate { get; set; }

        private string error;
        public override string Error
        {
            get
            {
                return "Error";
            }
        }

        public override string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                result = base[columnName];
                switch (columnName)
                {
                    case "Number":
                        if (Number <= 0) result = "Number must be greater than 0!"; break;
                    case "ProductType":
                        if (ProductType.Contains("a")) result = "Product type cannot contain 'a' letter"; break;
                    case "ReleaseDate":
                        if (ReleaseDate == default) result = "Release date must have a value"; break;
                };
                error = result;
                return result;
            }
        }

        public override string ToString()
        {
            return base.ToString() + string.Format(", {0}, {1}, {2}, Table={3}, Key={4}", Number, ProductType, ReleaseDate.ToString(), GetTableName(), GetKeyName());
        }

        public override string GetTableName()
        {
            return "Product";
        }

        public override string GetKeyName()
        {
            return "ProductId";
        }
    }
}
