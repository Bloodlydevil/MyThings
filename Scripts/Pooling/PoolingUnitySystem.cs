using MyThings.ExtendableClass;
using System.Collections.Generic;
using UnityEngine;

namespace MyThings.Pooling
{
    /// <summary>
    /// The Pooling Manager
    /// </summary>
    public class PoolingUnitySystem : Singleton_C<PoolingUnitySystem>
    {


        #region All The Pools


        /// <summary>
        /// The Dictionary for Storing Of all Pool 
        /// <code>
        /// Key is Name ( String )
        /// Value is Queue Of Inactive gameObject in Pool
        /// </code>
        /// </summary>
        private Dictionary<string, Pool<GameObject>> SystemPoolGameObject = new Dictionary<string, Pool<GameObject>>();
        /// <summary>
        /// The Dictionary for Storing Of all Pool 
        /// <code>
        /// Key is Name ( String )
        /// Value is Queue Of Component of gameObject in Pool
        /// </code>
        /// </summary>
        private Dictionary<string, Pool<Component>> SystemPoolComponent = new Dictionary<string, Pool<Component>>();

        #endregion


        #region Pool Create


        /// <summary>
        /// Function to create A new Pool
        /// </summary>
        /// <param name="Name">Name Of the pool</param>
        /// <param name="Count">The NO Of GameObject to Populate the scene before GameStart</param>
        /// <param name="PoolGameObject">The GameObject To create</param>
        /// <param name="Parent">The Parent Gameobject is attached to (When Returning the Object It Will Return Under it)</param>
        /// <returns>New Pool Was Created or not</returns>
        public bool TryCreateNewPool(string Name, int Count, GameObject PoolGameObject, Transform Parent=null)
        {
            if (SystemPoolGameObject.ContainsKey(Name))
            {
                Debug.LogError(Name + " is trying to create new pool when it already exist");
                return false;
            }
            else
            {
                Queue<GameObject> poolObject = new Queue<GameObject>(Count);
                for (int i = 0; i < Count; i++)
                {
                    GameObject Temp = Instantiate(PoolGameObject, Parent);
                    Temp.SetActive(false);

                    // try To Set Up Pooling Set Up
                    if (Temp.TryGetComponent(out IPoolingSetUp poolSetup))
                        poolSetup.SetUp();

                    poolObject.Enqueue(Temp);
                }
                // adding the created poolobject to system pool
                Pool<GameObject> pool = new Pool<GameObject>(PoolGameObject, poolObject, Parent);


                SystemPoolGameObject.Add(Name, pool);
            }
            return true;
        }



        /// <summary>
        /// Create A New Pool
        /// </summary>
        /// <typeparam name="t">The Component Type To Store</typeparam>
        /// <param name="Name">Name Of the pool</param>
        /// <param name="Count">The NO Of GameObject to Populate the scene before GameStart</param>
        /// <param name="PoolGameObject">The GameObject To create</param>
        /// <param name="Parent">The Parent Gameobject is attached to (When Returning the Object It Will Return Under it)</param>
        /// <returns>New Pool Was Created or not</returns>
        public bool TryCreateNewPool<t>(string Name, int Count, GameObject PoolGameObject, Transform Parent=null) where t : Component
        {
            if (SystemPoolComponent.ContainsKey(Name))
            {
                Debug.LogError(Name + " is trying to create new pool when it already exist");
                return false;
            }
            else
            {
                Queue<Component> poolObject = new Queue<Component>(Count);
                for (int i = 0; i < Count; i++)
                {
                    GameObject Temp = Instantiate(PoolGameObject, Parent);
                    Temp.SetActive(false);
                    if (Temp.TryGetComponent(out t comp))
                    {
                        poolObject.Enqueue(comp);
                    }
                    else
                    {
                        Debug.LogError("Error The Componenet Is Not Attached TO The gameObject");
                        return false;
                    }

                    // try To Set Up Pooling Set Up
                    if (Temp.TryGetComponent(out IPoolingSetUp poolSetup))
                        poolSetup.SetUp();
                }
                // adding the created poolobject to system pool
                SystemPoolComponent.Add(Name, new Pool<Component>(PoolGameObject, poolObject, Parent));
            }
            return true;
        }


        #endregion


        #region Destroy pool


        /// <summary>
        /// Function used to destroy the pool
        /// </summary>
        /// <param name="Name">Name of the Pool</param>
        /// <returns>Pool way destroyed or not</returns>
        public bool TryDestroyPool(string Name)
        {
            if (SystemPoolGameObject.ContainsKey(Name))
            {
                SystemPoolGameObject.Remove(Name);
                return true;
            }
            else
            {
                Debug.LogError(Name + " Pool does not exist");
                return false;
            }
        }

        /// <summary>
        ///  Function used to destroy the pool
        /// </summary>
        /// <typeparam name="t">The Component T</typeparam>
        /// <param name="Name">Name of the Pool</param>
        /// <returns>Pool way destroyed or not</returns>
        public bool TryDestroyPool<t>(string Name) where t : Component
        {
            if (SystemPoolComponent.ContainsKey(Name))
            {
                SystemPoolComponent.Remove(Name);
                return true;
            }
            else
            {
                Debug.LogError(Name + " Pool does not exist");
                return false;
            }
        }


        #endregion


        #region Spawn Object


        /// <summary>
        /// Function used to get object from pool or create one
        /// <code>
        /// GameObject will be in default state of inactive
        /// </code>
        /// </summary>
        /// <param name="Name">The Key of the GameObject to Get</param>
        /// <param name="pos">The Position of the object</param>
        /// <param name="rot">The Rotation Of the Object</param>'
        /// <param name="parent">The Parent To Attach To If Not Given The Default Parent Given Is Used</param>
        /// <returns>GameObject</returns>
        public GameObject SpawnGameObject(string Name, Vector3 pos, Quaternion rot,Transform parent=null)
        {
            // Try to Get the Pool if it exist
            // only true is there exist a pool of this name
            if (SystemPoolGameObject.TryGetValue(Name, out Pool<GameObject> pool))
            {
                // only give object if it does exist
                // else return it a brand new object
                if (pool._Queue.Count > 0)
                {
                    // setting up the GameObject
                    GameObject temp = pool._Queue.Dequeue();
                    if(parent != null)
                        temp.transform.SetParent(parent,false);
                    temp.transform.SetPositionAndRotation(pos, rot);
                    return temp;
                }
                else
                {
                    //Making sure GameObject is Not Active after creating
                    GameObject temp = Instantiate(pool._GameObject, pos, rot,parent==null?pool._transform:parent);

                    //try To Set up IPooling Set Up
                    if (temp.TryGetComponent(out IPoolingSetUp poolSetup))
                        poolSetup.SetUp();

                    temp.SetActive(false);
                    return temp;
                }
            }
            else
            {
                // If no pool exist then give message

                Debug.LogError("<color=red>Pool Does Not exist while spawning object</color>");

                return null;
            }
        }


        /// <summary>
        /// Function used to get Component from pool or create one
        /// <code>
        /// GameObject will be in default state of inactive
        /// </code>
        /// </summary>
        /// <typeparam name="t">The Component</typeparam>
        /// <param name="Name">The Key of the GameObject to Get</param>
        /// <param name="pos">The Position of the object</param>
        /// <param name="rot">The Rotation Of the Object</param>
        /// <returns>The Component</returns>
        public t SpawnGameObject<t>(string Name, Vector3 pos, Quaternion rot,Transform parent=null) where t : Component
        {
            // Try to Get the Pool if it exist
            // only true is there exist a pool of this name
            if (SystemPoolComponent.TryGetValue(Name, out Pool<Component> pool))
            {
                // only give object if it does exist
                // else return it a brand new object
                if (pool._Queue.Count > 0)
                {
                    // setting up the GameObject
                    t temp = (t)pool._Queue.Dequeue();
                    if (parent != null)
                        temp.transform.parent = parent;
                    temp.transform.SetPositionAndRotation(pos, rot);
                    return temp;
                }
                else
                {
                    //Making sure GameObject is Not Active after creating
                    GameObject temp = Instantiate(pool._GameObject, pos, rot, parent==null ? pool._transform : parent);
                    //try To Set up IPooling Set Up
                    if (temp.TryGetComponent(out IPoolingSetUp poolSetup))
                        poolSetup.SetUp();

                    temp.SetActive(false);
                    return temp.GetComponent<t>();
                }
            }
            else
            {
                // If no pool exist then give message

                Debug.LogError("<color=red>Pool Does Not exist while spawning object</color>   "+ Name+"    "+typeof(t));

                return null;
            }
        }


        #endregion


        #region Return Object


        /// <summary>
        /// Function is used to return the object to the pool
        /// </summary>
        /// <param name="Name">The Name of the Pool</param>
        /// <param name="obj">The Object to send to pool</param>
        public void ReturnObjectToPool(string Name, GameObject obj)
        {
            // making sure the gameobject is not active before putting it in pool
            obj.SetActive(false);

            // Try to Get the Pool if it exist
            // only true is there exist a pool of this name
            if (SystemPoolGameObject.TryGetValue(Name, out Pool<GameObject> pool))
            {
                obj.transform.parent = pool._transform;
                pool._Queue.Enqueue(obj);
            }
            else
            {
                // no pool exist
                Debug.LogError("<color=red>Pool Does Not exist while returning to pool</color>");
            }
        }


        /// <summary>
        /// Function is used to return the Component to the pool
        /// </summary>
        /// <typeparam name="t">The Component To Send To THe Pool</typeparam>
        /// <param name="Name">The Name of the Pool</param>
        /// <param name="obj">The Component to send to pool</param>
        public void ReturnObjectToPool<t>(string Name, t obj) where t : Component
        {
            // making sure the gameobject is not active before putting it in pool
            obj.gameObject.SetActive(false);

            // Try to Get the Pool if it exist
            // only true is there exist a pool of this name
            if (SystemPoolComponent.TryGetValue(Name, out Pool<Component> pool))
            {
                obj.transform.parent = pool._transform;
                pool._Queue.Enqueue(obj);
            }
            else
            {
                // no pool exist
                Debug.LogError("<color=red>Pool Does Not exist while returning to pool</color>");
            }
        }

        #endregion
    }
}