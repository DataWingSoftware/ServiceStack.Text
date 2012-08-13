using System;
using System.Collections.Generic;
using System.IO;
using ServiceStack.Text.Common;
using ServiceStack.Text.Json;
using ServiceStack.Text.Jsv;
#if WINDOWS_PHONE
using ServiceStack.Text.WP;
#endif

namespace ServiceStack.Text
{
	public static class
		JsConfig
	{
		static JsConfig()
		{
			//In-built default serialization, to Deserialize Color struct do:
			//JsConfig<System.Drawing.Color>.SerializeFn = c => c.ToString().Replace("Color ", "").Replace("[", "").Replace("]", "");
			//JsConfig<System.Drawing.Color>.DeSerializeFn = System.Drawing.Color.FromName;
            Reset();
		}
        
		[ThreadStatic]
		private static bool? tsConvertObjectTypesIntoStringDictionary;
		private static bool? sConvertObjectTypesIntoStringDictionary;
		public static bool ConvertObjectTypesIntoStringDictionary
		{
			get
			{
				return tsConvertObjectTypesIntoStringDictionary ?? sConvertObjectTypesIntoStringDictionary ?? false;
			}
			set
			{
				tsConvertObjectTypesIntoStringDictionary = value;
				if (!sConvertObjectTypesIntoStringDictionary.HasValue) sConvertObjectTypesIntoStringDictionary = value;
			}
		}

		[ThreadStatic]
		private static bool? tsIncludeNullValues;
		private static bool? sIncludeNullValues;
		public static bool IncludeNullValues
		{
			get
			{
				return tsIncludeNullValues ?? sIncludeNullValues ?? false;
			}
			set
			{
				tsIncludeNullValues = value;
				if (!sIncludeNullValues.HasValue) sIncludeNullValues = value;
			}
		}

		[ThreadStatic]
		private static bool? tsExcludeTypeInfo;
		private static bool? sExcludeTypeInfo;
		public static bool ExcludeTypeInfo
		{
			get
			{
				return tsExcludeTypeInfo ?? sExcludeTypeInfo ?? false;
			}
			set
			{
				tsExcludeTypeInfo = value;
				if (!sExcludeTypeInfo.HasValue) sExcludeTypeInfo = value;
			}
		}

		[ThreadStatic]
		private static JsonDateHandler? tsDateHandler;
		private static JsonDateHandler? sDateHandler;
		public static JsonDateHandler DateHandler
		{
			get
			{
				return tsDateHandler ?? sDateHandler ?? JsonDateHandler.TimestampOffset;
			}
			set
			{
				tsDateHandler = value;
				if (!sDateHandler.HasValue) sDateHandler = value;
			}
		}

        /// <summary>
        /// Sets which format to use when serializing TimeSpans
        /// </summary>
        public static JsonTimeSpanHandler TimeSpanHandler { get; set; }

		/// <summary>
		/// <see langword="true"/> if the <see cref="ITypeSerializer"/> is configured
		/// to take advantage of <see cref="CLSCompliantAttribute"/> specification,
		/// to support user-friendly serialized formats, ie emitting camelCasing for JSON
		/// and parsing member names and enum values in a case-insensitive manner.
		/// </summary>
		[ThreadStatic]
		private static bool? tsEmitCamelCaseNames;
		private static bool? sEmitCamelCaseNames;
		public static bool EmitCamelCaseNames
		{
			// obeying the use of ThreadStatic, but allowing for setting JsConfig once as is the normal case
			get
			{
				return tsEmitCamelCaseNames ?? sEmitCamelCaseNames ?? false;
			}
			set
			{
				tsEmitCamelCaseNames = value;
				if (!sEmitCamelCaseNames.HasValue) sEmitCamelCaseNames = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating if the framework should throw serialization exceptions
		/// or continue regardless of deserialization errors. If <see langword="true"/>  the framework
		/// will throw; otherwise, it will parse as many fields as possible. The default is <see langword="false"/>.
		/// </summary>
		[ThreadStatic]
		private static bool? tsThrowOnDeserializationError;
		private static bool? sThrowOnDeserializationError;
		public static bool ThrowOnDeserializationError
		{
			// obeying the use of ThreadStatic, but allowing for setting JsConfig once as is the normal case
			get
			{
				return tsThrowOnDeserializationError ?? sThrowOnDeserializationError ?? false;
			}
			set
			{
				tsThrowOnDeserializationError = value;
				if (!sThrowOnDeserializationError.HasValue) sThrowOnDeserializationError = value;
			}
		}

        internal static HashSet<Type> HasSerializeFn = new HashSet<Type>();

        internal static HashSet<Type> TreatValueAsRefTypes = new HashSet<Type>();

        internal static bool TreatAsRefType(Type valueType)
        {
            return TreatValueAsRefTypes.Contains(valueType.IsGenericType ? valueType.GetGenericTypeDefinition() : valueType);
        }

	    public static void Reset()
		{
			tsConvertObjectTypesIntoStringDictionary = sConvertObjectTypesIntoStringDictionary = null;
			tsIncludeNullValues = sIncludeNullValues = null;
			tsExcludeTypeInfo = sExcludeTypeInfo = null;
			tsEmitCamelCaseNames = sEmitCamelCaseNames = null;
			tsDateHandler = sDateHandler = null;
			tsThrowOnDeserializationError = sThrowOnDeserializationError = null;
            HasSerializeFn = new HashSet<Type>();
            TreatValueAsRefTypes = new HashSet<Type> {
                typeof(KeyValuePair<,>)
            };
		}

#if MONOTOUCH
        /// <summary>
        /// Provide hint to MonoTouch AOT compiler to pre-compile generic classes for all your DTOs.
        /// Just needs to be called once in a static constructor.
        /// </summary>
        [MonoTouch.Foundation.Preserve]
		public static void InitForAot() { }

        [MonoTouch.Foundation.Preserve]
        public static void RegisterForAot()
        {
            JsonAotConfig.Register<Poco>();

            RegisterElement<Poco, string>();

            RegisterElement<Poco, bool>();
            RegisterElement<Poco, char>();
            RegisterElement<Poco, byte>();
            RegisterElement<Poco, sbyte>();
            RegisterElement<Poco, short>();
            RegisterElement<Poco, ushort>();
            RegisterElement<Poco, int>();
            RegisterElement<Poco, uint>();
            RegisterElement<Poco, long>();
            RegisterElement<Poco, ulong>();
            RegisterElement<Poco, float>();
            RegisterElement<Poco, double>();
            RegisterElement<Poco, decimal>();
            RegisterElement<Poco, Guid>();
            RegisterElement<Poco, DateTime>();
            RegisterElement<Poco, TimeSpan>();

            RegisterElement<Poco, bool?>();
            RegisterElement<Poco, char?>();
            RegisterElement<Poco, byte?>();
            RegisterElement<Poco, sbyte?>();
            RegisterElement<Poco, short?>();
            RegisterElement<Poco, ushort?>();
            RegisterElement<Poco, int?>();
            RegisterElement<Poco, uint?>();
            RegisterElement<Poco, long?>();
            RegisterElement<Poco, ulong?>();
            RegisterElement<Poco, float?>();
            RegisterElement<Poco, double?>();
            RegisterElement<Poco, decimal?>();
            RegisterElement<Poco, Guid?>();
            RegisterElement<Poco, DateTime?>();
            RegisterElement<Poco, TimeSpan?>();

            RegisterQueryStringWriter();
            RegisterCsvSerializer();
        }

		[MonoTouch.Foundation.Preserve]
		public static bool RegisterTypeForAot<T>()
		{
			bool ret = false;
			try
			{
				JsonAotConfig.Register<T>();

				int i = 0;
				if(JsvWriter<T>.WriteFn() != null && JsvReader<T>.GetParseFn() != null) i++;
				if(JsonWriter<T>.WriteFn() != null && JsonReader<T>.GetParseFn() != null) i++;
				if(QueryStringWriter<Poco>.WriteFn() != null) i++;

				CsvSerializer<T>.WriteFn();
	            CsvSerializer<T>.WriteObject(null, null);
	            CsvWriter<T>.WriteObject(null, null);
	            CsvWriter<T>.WriteObjectRow(null, null);
				ret = true;
			}catch(Exception){}

			return ret;
		}

        [MonoTouch.Foundation.Preserve]
        static void RegisterQueryStringWriter()
        {
            var i = 0;
            if (QueryStringWriter<Poco>.WriteFn() != null) i++;
        }

        [MonoTouch.Foundation.Preserve]
        static void RegisterCsvSerializer()
        {
            CsvSerializer<Poco>.WriteFn();
            CsvSerializer<Poco>.WriteObject(null, null);
            CsvWriter<Poco>.WriteObject(null, null);
            CsvWriter<Poco>.WriteObjectRow(null, null);
        }

        [MonoTouch.Foundation.Preserve]
        public static void RegisterElement<T, TElement>()
        {
            JsonAotConfig.RegisterElement<T, TElement>();
        }
#endif

	}

#if MONOTOUCH
    [MonoTouch.Foundation.Preserve(AllMembers=true)]
    internal class Poco
    {
        public string Dummy { get; set; }
    }

    [MonoTouch.Foundation.Preserve(AllMembers=true)]
    internal class JsonAotConfig
    {
        static JsReader<JsonTypeSerializer> reader;
        static JsWriter<JsonTypeSerializer> writer;
        static JsonTypeSerializer serializer;

        static JsonAotConfig()
        {
            serializer = new JsonTypeSerializer();
            reader = new JsReader<JsonTypeSerializer>();
            writer = new JsWriter<JsonTypeSerializer>();
        }

        public static ParseStringDelegate GetParseFn(Type type)
        {
            var parseFn = JsonTypeSerializer.Instance.GetParseFn(type);
            return parseFn;
        }

        internal static ParseStringDelegate RegisterBuiltin<T>()
        {
            var i = 0;
            if (reader.GetParseFn<T>() != null) i++;
            if (JsonReader<T>.GetParseFn() != null) i++;
            if (JsonReader<T>.Parse(null) != null) i++;
            if (JsonWriter<T>.WriteFn() != null) i++;

            return serializer.GetParseFn<T>();
        }

        public static void Register<T>()
        {
            var i = 0;
            var serializer = JsonTypeSerializer.Instance;
            if (new List<T>() != null) i++;
            if (new T[0] != null) i++;
            if (serializer.GetParseFn<T>() != null) i++;
            if (DeserializeArray<T[], JsonTypeSerializer>.Parse != null) i++;

            JsConfig<T>.ExcludeTypeInfo = false;
            //JsConfig<T>.SerializeFn = arg => "";
            //JsConfig<T>.DeSerializeFn = arg => default(T);

            DeserializeArrayWithElements<T, JsonTypeSerializer>.ParseGenericArray(null, null);
            DeserializeCollection<JsonTypeSerializer>.ParseCollection<T>(null, null, null);
            DeserializeListWithElements<T, JsonTypeSerializer>.ParseGenericList(null, null, null);

            SpecializedQueueElements<T>.ConvertToQueue(null);
            SpecializedQueueElements<T>.ConvertToStack(null);

            WriteListsOfElements<T, JsonTypeSerializer>.WriteList(null, null);
            WriteListsOfElements<T, JsonTypeSerializer>.WriteIList(null, null);
            WriteListsOfElements<T, JsonTypeSerializer>.WriteEnumerable(null, null);
            WriteListsOfElements<T, JsonTypeSerializer>.WriteListValueType(null, null);
            WriteListsOfElements<T, JsonTypeSerializer>.WriteIListValueType(null, null);

            JsonReader<T>.Parse(null);
            JsonWriter<T>.WriteFn();

            TranslateListWithElements<T>.LateBoundTranslateToGenericICollection(null, null);
            TranslateListWithConvertibleElements<T, T>.LateBoundTranslateToGenericICollection(null, null);

            QueryStringWriter<T>.WriteObject(null, null);
        }

        // Edited to fix issues with null List<Guid> properties in response objects
        public static void RegisterElement<T, TElement>()
        {
            RegisterBuiltin<TElement>();
            DeserializeDictionary<JsonTypeSerializer>.ParseDictionary<T, TElement>(null, null, null, null);
            DeserializeDictionary<JsonTypeSerializer>.ParseDictionary<TElement, T>(null, null, null, null);

            ToStringDictionaryMethods<T, TElement, JsonTypeSerializer>.WriteIDictionary(null, null, null, null);
            ToStringDictionaryMethods<TElement, T, JsonTypeSerializer>.WriteIDictionary(null, null, null, null);

            // Include List deserialisations from the Register<> method above.  This solves issue where List<Guid> properties on responses deserialise to null.
            // No idea why this is happening because there is no visible exception raised.  Suspect MonoTouch is swallowing an AOT exception somewhere.
            DeserializeArrayWithElements<TElement, JsonTypeSerializer>.ParseGenericArray(null, null);
            DeserializeListWithElements<TElement, JsonTypeSerializer>.ParseGenericList(null, null, null);

            // Cannot use the line below for some unknown reason - when trying to compile to run on device, mtouch bombs during native code compile.
            // Something about this line or its inner workings is offensive to mtouch. Luckily this was not needed for my List<Guide> issue.
            // DeserializeCollection<JsonTypeSerializer>.ParseCollection<TElement>(null, null, null);

            TranslateListWithElements<TElement>.LateBoundTranslateToGenericICollection(null, typeof(List<TElement>));
            TranslateListWithConvertibleElements<TElement, TElement>.LateBoundTranslateToGenericICollection(null, typeof(List<TElement>));
        }
    }
#endif

    public class JsConfig<T> //where T : struct
	{
		/// <summary>
		/// Never emit type info for this type
		/// </summary>
		public static bool ExcludeTypeInfo = false;

		/// <summary>
		/// <see langword="true"/> if the <see cref="ITypeSerializer"/> is configured
		/// to take advantage of <see cref="CLSCompliantAttribute"/> specification,
		/// to support user-friendly serialized formats, ie emitting camelCasing for JSON
		/// and parsing member names and enum values in a case-insensitive manner.
		/// </summary>
		public static bool EmitCamelCaseNames = false;

		/// <summary>
		/// Define custom serialization fn for BCL Structs
		/// </summary>
		private static Func<T, string> serializeFn;
		public static Func<T, string> SerializeFn
		{
			get { return serializeFn; }
			set
			{
				serializeFn = value;
				if (value != null)
					JsConfig.HasSerializeFn.Add(typeof(T));
				else
					JsConfig.HasSerializeFn.Remove(typeof(T));
			}
		}

        /// <summary>
        /// Opt-in flag to set some Value Types to be treated as a Ref Type
        /// </summary>
        public bool TreatValueAsRefTypes
	    {
	        get { return JsConfig.TreatValueAsRefTypes.Contains(typeof (T)); }
	        set
	        {
                if (value)
	                JsConfig.TreatValueAsRefTypes.Add(typeof(T));
                else
                    JsConfig.TreatValueAsRefTypes.Remove(typeof(T));
            }
	    }

		/// <summary>
		/// Define custom deserialization fn for BCL Structs
		/// </summary>
		public static Func<string, T> DeSerializeFn;

		/// <summary>
		/// Exclude specific properties of this type from being serialized
		/// </summary>
		public static string[] ExcludePropertyNames;

		public static void WriteFn<TSerializer>(TextWriter writer, object obj)
		{
			var serializer = JsWriter.GetTypeSerializer<TSerializer>();
			serializer.WriteString(writer, SerializeFn((T)obj));
		}

		public static object ParseFn(string str)
		{
			return DeSerializeFn(str);
		}
	}

	public enum JsonDateHandler
	{
		TimestampOffset,
		DCJSCompatible,
		ISO8601
	}

    public enum JsonTimeSpanHandler
    {
        /// <summary>
        /// Uses the xsd format like PT15H10M20S
        /// </summary>
        DurationFormat,
        /// <summary>
        /// Uses the standard .net ToString method of the TimeSpan class
        /// </summary>
        StandardFormat
    }
}

