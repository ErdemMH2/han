using HAN.Lib.Basic;
using System.Collections.Generic;

namespace HAN.Lib.Mvc.Model
{
    /**
     * The IHANModel is the most basic model specification. IncrementalIds will be used to query the model.
     * All model interfaceses should implement IHANModel to provide generic access.
     * Specialization as the TableModel or ListModel can provide unified access for controls.
     * 
     * A IHANModel should be used with the ModelEntry. By using a Incremental Id a ModelEntry is retrieved.
     * This link will connect directly to the data. The model will use it to communicate changes to the consumer.
     * 
     * The Data can also be fetched without a ModelEntryLink. But this should only be used if it will used only for 
     * a short time. Communicating over the ModelEntryLink will prevent expensive searchs.
     */
    public interface IHANModel<I, T> : IHANObject
                                       where I : IncrementalId
    {
        IEnumerable<T> Values { get; }

        IEnumerable<ModelEntry<I, T>> Entrys { get; }

        /**
         * Returns ModelEntry which represents the position and data
         * The data can change and we can directly observe it
         */
        ModelEntry<I, T> Position( I a_key );

        /**
         * One time usage of data. No link is provided. Do not overuse!
         * It will try to return the value or null on data not found or cast fail
         */
        T Value( I a_key );
    }


    /**
     * Interface for writable models. Can be used to extend readonly models.
     */
    public interface IHANWriteableModel<I, T> : IHANModel<I, T>
                                                where I : IncrementalId
    {
        /**
         * Will update an existing entry with the entries values, ModelId remains the same.
         */
        bool Assign( I a_key, T a_entry );


        /**
         * Inserts a entry to the model
         */
        bool Insert( I a_key, T a_entry );


        /**
         * Inserts a entry to the model
         */
        I Append( T a_entry );


        /**
         * Removes an entry with the Modelid a_modelId
         */
        bool Remove( I a_key );
    }
}
 