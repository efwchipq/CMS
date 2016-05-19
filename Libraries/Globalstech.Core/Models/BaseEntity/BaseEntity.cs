using System;
using System.ComponentModel;

namespace Globalstech.Core.Models.BaseEntity {

    public class BaseEntity : IDEntity {

        private DateTime _createdOnDate;

        /// <summary>
        /// 创建日期
        /// </summary>
        public virtual DateTime CreatedOnDate {
            get {
                if (_createdOnDate == DateTime.MinValue) {
                    _createdOnDate = DateTime.Now;
                }
                return _createdOnDate;
            }
            set {
                _createdOnDate = value;
            }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        [DefaultValue(-1)]
        public virtual int CreatedByUserID {
            get;
            set;
        }

        /// <summary>
        /// 修改日期
        /// </summary>
        public virtual DateTime? LastModifiedOnDate {
            get;
            set;
        }

        /// <summary>
        /// 修改人
        /// </summary>
        public virtual int? LastModifiedByUserID {
            get;
            set;
        }

        /// <summary>
        /// 是否已删除
        /// </summary>
        [DefaultValue(false)]
        public virtual bool IsDeleted {
            get;
            set;
        }

    }

}
