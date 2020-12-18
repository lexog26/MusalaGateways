using System;
using System.Collections.Generic;
using System.Text;

namespace MusalaGateways.Domain.Entities
{
    public class Entity<T>
    {
        private DateTime? _createdDate;

        public T Id { get; set; }

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreatedDate
        {
            get { return _createdDate ?? DateTime.UtcNow; }
            set { _createdDate = value; }
        }

        /// <summary>
        /// Last modification date
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// TimeStamp for create/update db contexts
        /// </summary>
        public DateTime LastTimeStamp { get; set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}
