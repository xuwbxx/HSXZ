using System;

namespace TrainingsApp.Helper
{
    public class EnumItem
    {
        private object m_key;
        private object m_value;


        public EnumItem() { }

        public EnumItem(object _key, object _value)
        {
            this.m_key = _key;
            this.m_value = _value;
        }

        public override string ToString()
        {
            return string.Format("{0}-{1}", this.m_value, this.m_key);
        }

        public object Key
        {
            get
            {
                return this.m_key;
            }
            set
            {
                this.m_key = value;
            }
        }

        public object Value
        {
            get
            {
                return this.m_value;
            }
            set
            {
                this.m_value = value;
            }
        }
    }
}

