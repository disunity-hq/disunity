using System;


namespace Disunity.Store.RouteConstraints {

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RouteConstraintAttribute : Attribute {

        public string ConstraintName { get; set; }

        public RouteConstraintAttribute(string constraintName) {
            ConstraintName = constraintName;

        }

    }

}