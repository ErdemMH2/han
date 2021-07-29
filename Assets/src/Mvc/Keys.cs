using HAN.Lib.Structure;


namespace HAN.Lib.Mvc
{ 
    public static class Keys
    {
        public static ComponentKeys Component { get { return ComponentKeys.Instance; } }
        public class ComponentKeys : Singleton<ComponentKeys>
        {
            public readonly Key Id       = KeyFactory.Create("Id");
            public readonly Key Enabled  = KeyFactory.Create("Enabled");
            public readonly Key ObjectEnabled  = KeyFactory.Create("ObjectEnabled");
            public readonly Key ObjectDestroyed  = KeyFactory.Create("ObjectDestroyed");
        }


        public static TransformKeys Transform { get { return TransformKeys.Instance; } }
        public class TransformKeys : Singleton<TransformKeys>
        {
            public readonly Key Position = KeyFactory.Create("Position");
            public readonly Key Size     = KeyFactory.Create("Size");
            public readonly Key Rotation = KeyFactory.Create("Rotation");
            public readonly Key Scale    = KeyFactory.Create("Scale");
            public readonly Key Corners  = KeyFactory.Create("Corners");
        }


        public static ModelKeys Model { get { return ModelKeys.Instance; } }
        public class ModelKeys : Singleton<ModelKeys>
        {
            public readonly Key Data = KeyFactory.Create( "Data" );

            public readonly Key Insert = KeyFactory.Create( "Insert" );
            public readonly Key Append = KeyFactory.Create( "Append" );

            public readonly Key Assign = KeyFactory.Create( "Assign" );
            public readonly Key Add    = KeyFactory.Create( "Add" );
            public readonly Key Remove = KeyFactory.Create( "Remove" );
        }
    }
}