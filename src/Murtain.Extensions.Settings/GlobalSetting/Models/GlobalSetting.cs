﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Murtain.Extensions.Settings.GlobalSetting.Models
{
    public class GlobalSetting
    {
        public GlobalSetting()
        {
            this.Scope = GlobalSettingScope.Application;
        }

        public GlobalSetting(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public GlobalSetting(string displayName, string name, string value, string group, string description, GlobalSettingScope scope)
            : this(name, value)
        {
            this.DisplayName = displayName;
            this.Group = group;
            this.Description = description;
            this.Scope = scope;
        }

        public long Id { get; set; }

        /// <summary>
        /// Unique name of the Setting.
        /// </summary>
        [Required]
        [MaxLength(256)]
        public virtual string Name { get; set; }
        /// <summary>
        /// Display Name
        /// </summary>
        [Required]
        [MaxLength(256)]
        public virtual string DisplayName { get; set; }
        /// <summary>
        /// Value of the setting.
        /// </summary>
        [MaxLength(2000)]
        public virtual string Value { get; set; }
        /// <summary>
        /// Description
        /// </summary>
        [MaxLength(2000)]
        public virtual string Description { get; set; }
        /// <summary>
        /// Scopes of this setting.
        /// Default value: <see cref="GlobalSettingScope.Application"/>.
        /// </summary>
        public virtual GlobalSettingScope Scope { get; set; }
        /// <summary>
        /// GlobalSetting Group
        /// </summary>
        public virtual string Group { get; set; }
    }
}
