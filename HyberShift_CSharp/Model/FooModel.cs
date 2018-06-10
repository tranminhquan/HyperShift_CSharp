namespace HyberShift_CSharp.Model
{
    public class FooModel
    {
        // attribute of model

        // getter and setter
        public string Attribute1 { get; set; }

        public double Attribute2 { get; set; }

        public bool IsFoo { get; set; }

        //methods of model
        public void Method1()
        {
        }

        public double Method2()
        {
            return 0f;
        }

        public bool IsValidAttribute1() //example about data validation
        {
            return true;
        }

        // others method ...
    }
}