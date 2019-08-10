using System;
using System.ComponentModel.DataAnnotations;

using EFCoreHooks.Attributes;


namespace Disunity.Store.Data {

    public interface ICreatedAt {

        [DataType(DataType.DateTime)]
        [Display(Name="Created At")]
        DateTime CreatedAt { get; set; }

    }
    
    public static class CreatedAtHooks {

        [OnBeforeCreate(typeof(ICreatedAt))]
        public static void SetCreatedAt(ICreatedAt model) {
            model.CreatedAt = DateTime.Now;
        }
    }

}