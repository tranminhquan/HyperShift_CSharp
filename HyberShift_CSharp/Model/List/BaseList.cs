using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HyberShift_CSharp.Model.List
{
    public abstract class BaseList<T>
    {
        protected ObservableCollection<T> list;

        // constructor
        public BaseList()
        {
            list = new ObservableCollection<T>();
        }

        // getter and setter
        public ObservableCollection<T> List
        {
            get => list;
            set
            {
                list.Clear();
                list = value;
            }
        }

        // method
        /// <summary>
        ///     Lấy ra một collections thõa mãn giá trị cho trước thông qua một thuộc tính
        ///     Vd: list chứa thông tin của những UserOnline,
        ///     UserOnline gồm các thuộc thuộc tính Name, Email
        ///     => GetCollectionOfField("Name", "Tran Minh Quan") trả về một collection chứa các UserOnline có Name là "Tran Minh
        ///     Quan" của list
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ObservableCollection<T> GetCollectionByValue(string fieldName, dynamic value)
        {
            var result = new ObservableCollection<T>();
            foreach (var obj in list)
                if (GetPropertyValue(obj, fieldName) == value)
                    result.Add(obj);
            return result;
        }

        /// <summary>
        ///     Lấy ra đối tượng đầu tiên thõa mãn giá trị cho trước thông qua một thuộc tính
        ///     Vd: list chứa thông tin của những UserOnline,
        ///     UserOnline gồm các thuộc thuộc tính Name, Email
        ///     => GetCollectionOfField("Name", "Tran Minh Quan") trả về một đối tượng UserOnline có Name là "Tran Minh Quan" của
        ///     list
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public T GetFirstObjectByValue(string fieldName, dynamic value)
        {
            foreach (var obj in list)
                if (GetPropertyValue(obj, fieldName) == value)
                    return obj;
            return default(T);
        }

        /// <summary>
        ///     Lấy ra một collections chứa các giá trị của một thuộc tính
        ///     Vd: list chứa thông tin của những UserOnline,
        ///     UserOnline gồm các thuộc thuộc tính Name, Email
        ///     => GetCollectionOfField("Name") trả về một collection chứa các Name của list
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public ObservableCollection<dynamic> GetCollectionOfField(string fieldName)
        {
            var result = new ObservableCollection<dynamic>();
            foreach (var obj in list) result.Add(GetPropertyValue(obj, fieldName));
            return result;
        }

        /// <summary>
        ///     Lấy ra một collections chứa các giá trị của một thuộc tính nếu thõa mãn giá trị value
        ///     Vd: list chứa thông tin của những UserOnline,
        ///     UserOnline gồm các thuộc thuộc tính Name, Email
        ///     => GetCollectionOfFieldByValue("Name", "Tran Minh Quan") trả về một collection chứa các Name của list có giá trị là
        ///     "Tran Minh Quan"
        /// </summary>
        /// <param name="fieldNameToGet">tên thuộc tính cần lấy</param>
        /// <param name="fieldNameToCompare">tên thuộc tính cần đối chiếu</param>
        /// <param name="value">giá trị thuộc tính cần đối chiếu</param>
        /// <returns></returns>
        public ObservableCollection<dynamic> GetCollectionOfFieldByValue(string fieldNameToGet,
            string fieldNameToCompare, dynamic value)
        {
            var result = new ObservableCollection<dynamic>();
            foreach (var obj in list)
                if (GetPropertyValue(obj, fieldNameToCompare) == value)
                    result.Add(GetPropertyValue(obj, fieldNameToGet));
            return result;
        }

        /// <summary>
        ///     Lấy ra một đối tượng chứa các giá trị của một thuộc tính nếu thõa mãn giá trị value
        ///     Vd: list chứa thông tin của những UserOnline,
        ///     UserOnline gồm các thuộc thuộc tính Name, Email
        ///     => GetCollectionOfFieldByValue("Name", "Tran Minh Quan") trả về một đối tượng chứa các Name của list có giá trị là
        ///     "Tran Minh Quan"
        /// </summary>
        /// <param name="fieldNameToGet"></param>
        /// <param name="fieldNameToCompare"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public dynamic GetFirstObjectOfFieldByValue(string fieldNameToGet, string fieldNameToCompare, dynamic value)
        {
            foreach (var obj in list)
                if (GetPropertyValue(obj, fieldNameToCompare) == value)
                    return obj;
            return null;
        }

        public int GetIndexByValue(string fieldNameToCompare, dynamic value)
        {
            for (var i = 0; i < list.Count; i++)
                if (GetPropertyValue(list[i], fieldNameToCompare) == value)
                    return i;
            return -1;
        }

        public dynamic GetFieldValueByIndex(string fieldNameToGet, int index)
        {
            return GetPropertyValue(list[index], fieldNameToGet);
        }

        /// <summary>
        ///     Thêm phần tử vào list (KHÔNG kiểm tra trùng lắp)
        /// </summary>
        /// <param name="obj">object cần thêm</param>
        public void Add(T obj)
        {
            list.Add(obj);
        }

        /// <summary>
        ///     Thêm phần tử vào list (có kiểm tra trùng lắp)
        /// </summary>
        /// <param name="obj">object cần thêm</param>
        /// <param name="propertyName">tên thuộc tính cần kiểm tra</param>
        public void AddWithCheck(T obj, string propertyName)
        {
            foreach (var element in list)
                // if duplicated
                if (GetPropertyValue(element, propertyName).Equals(GetPropertyValue(obj, propertyName)))
                    return;
            list.Add(obj);
        }

        public void UpdateWithCheck(T obj, string checkProperty)
        {
            foreach (var element in list)
            {
                if (GetPropertyValue(element, checkProperty).Equals(GetPropertyValue(obj, checkProperty)))
                {
                    list.Remove(element);
                    list.Add(obj);
                }
            }
        }

        public void AddWithCheck(T obj, params string[] propertyNames)
        {
            foreach (var element in list)
                // if duplicated
                foreach(string property in propertyNames)
                    if (GetPropertyValue(element, property).Equals(GetPropertyValue(obj, property)))
                        return;
            list.Add(obj);
        }

        /// <summary>
        ///     Lấy giá trị của thuộc tính
        /// </summary>
        /// <param name="obj">object cần lấy thuộc tính</param>
        /// <param name="propertyName">tên thuộc tính kiểu string</param>
        /// <returns>Kiểu object chứa giá trị của thuộc tính đó</returns>
        public static dynamic GetPropertyValue(dynamic obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj);
        }
    }
}