using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Your.Entity
{
    public abstract class IEntity<TKey>
    {
        [Key]
        public TKey ID { get; set; }

        //audit trail related common properties
        private DateTime _editdate;
        public DateTime EditDate
        {
            get
            {
                if (_editdate == DateTime.MinValue)
                    _editdate = DateTime.Now;
                return _editdate;
            }
            set { _editdate = value; }
        }
        private DateTime _createdate;
        public DateTime CreateDate
        {
            get
            {
                if (_createdate == DateTime.MinValue)
                    _createdate = DateTime.Now;
                return _createdate;
            }
            set { _createdate = value; }
        }

        private int? _isactive;
        public int? IsActive
        {
            get
            {
                if (!_isactive.HasValue)
                    _isactive = 1;
                return _isactive;
            }
            set { _isactive = value; }
        }
    }
}
