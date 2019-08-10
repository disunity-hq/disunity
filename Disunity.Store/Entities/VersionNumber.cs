using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Disunity.Store.Entities {

    public class VersionNumber : IComparable<VersionNumber> {

        public int ID { get; set; }

        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }

        public VersionNumber() : this(1) { }

        public VersionNumber(int major, int minor = 0, int patch = 0) {
            Major = major;
            Minor = minor;
            Patch = patch;
        }
        
        public static VersionNumber Create(string versionNumber) {
            if (versionNumber == null) {
                return null;
            }

            var segments = versionNumber.Split('.');

            if (segments.Length != 3) {
                throw new InvalidOperationException(
                    $"versionNumber must be of format MAJOR.MINOR.PATCH instead {versionNumber} was provided");
            }

            return new VersionNumber {

                Major = int.Parse(segments[0]),
                Minor = int.Parse(segments[1]),
                Patch = int.Parse(segments[2])
            };
        }

        public static implicit operator string(VersionNumber v) {
            return v?.ToString();
        }

        public static explicit operator VersionNumber(string s) {
            return Create(s);
        }

        public override string ToString() {
            return $"{Major}.{Minor}.{Patch}";
        }

        public override bool Equals(object obj) {
            return CompareTo(obj as VersionNumber) == 0;
        }

        protected bool Equals(VersionNumber other) {
            return Major == other.Major && Minor == other.Minor && Patch == other.Patch;
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = Major;
                hashCode = (hashCode * 397) ^ Minor;
                hashCode = (hashCode * 397) ^ Patch;
                return hashCode;
            }
        }

        public int CompareTo(VersionNumber other) {
            if (ReferenceEquals(this, other)) {
                return 0;
            }

            if (ReferenceEquals(null, other)) {
                return 1;
            }

            var majorComparison = Major.CompareTo(other.Major);

            if (majorComparison != 0) {
                return majorComparison;
            }

            var minorComparison = Minor.CompareTo(other.Minor);

            if (minorComparison != 0) {
                return minorComparison;
            }

            return Patch.CompareTo(other.Patch);
        }

        public class VersionNumberConfiguration : IEntityTypeConfiguration<VersionNumber> {

            public void Configure(EntityTypeBuilder<VersionNumber> builder) {
                builder.HasAlternateKey(v => new {v.Major, v.Minor, v.Patch});
            }

        }

    }

}