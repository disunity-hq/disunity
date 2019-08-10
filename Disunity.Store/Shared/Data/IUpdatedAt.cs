using System;
using System.ComponentModel.DataAnnotations;

using EFCoreHooks.Attributes;


namespace Disunity.Store.Data {

    public interface IUpdatedAt {

        [DataType(DataType.DateTime)]
        [Display(Name = "Created At")]

        DateTime UpdatedAt { get; set; }

    }

    public static class UpdatedAtHooks {

        [OnBeforeSave(typeof(IUpdatedAt))]
        public static void SetCreatedAt(IUpdatedAt model) {
            model.UpdatedAt = DateTime.Now;
        }

    }

}