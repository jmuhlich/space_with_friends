using System;
using System.Collections.Generic;

namespace space_with_friends
{
    public class Lock : IEquatable<Lock>, ICloneable
    {
        /// <summary>
        /// Lock ID
        /// </summary>
        public Guid id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Lock owner
        /// </summary>
        public string owner { get; set; } = string.Empty;

        /// <summary>
        /// Vessel id assigned to the lock. Can be Guid.Empty
        /// </summary>
        public Guid vessel_id { get; set; } = Guid.Empty;

        /// <summary>
        /// The type of the lock
        /// </summary>
        public string type { get; set; } = string.Empty;

        internal Lock()
        {
        }

        public override string ToString()
        {
            return $"LOCK: id: { id } owner: { owner } vessel_id: { vessel_id } type: ${ type }";
        }

        public object Clone()
        {
            return new Lock
            {
                id = id,
                owner = owner.Clone() as string,
                vessel_id = vessel_id,
                type = type.Clone() as string
            };
        }

        public void Serialize()
        {
        }

        public void Deserialize()
        {
        }

        public int GetByteCount()
        {
			return 0;
        }

        #region Equatable
        public bool Equals(Lock other)
        {
            if (other == null)
                return false;

            return id == other.id && owner == other.owner && vessel_id == other.vessel_id && type == other.type;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var lockObj = obj as Lock;
            return lockObj != null && Equals(lockObj);
        }

        public override int GetHashCode()
        {
            var hashCode = -423896247;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(owner);
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(vessel_id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(type);
            return hashCode;
        }

        public static bool operator ==(Lock lhs, Lock rhs)
        {
            // why is this case necessary? why can't we just return Equals( lhs, rhs )?
            if ((object)lhs == null || (object)rhs == null)
                return Equals(lhs, rhs);

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Lock lhs, Lock rhs)
        {
            if ((object)lhs == null || (object)rhs == null)
                return !Equals(lhs, rhs);

            return !lhs.Equals(rhs);
        }

        #endregion
    }
}
