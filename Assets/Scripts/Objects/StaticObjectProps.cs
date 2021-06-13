using UnityEngine;

public class StaticObjectProps : MonoBehaviour {
     public bool immovable;
     public bool deadly;

     private Vector2 startPos;

     private void Start() {
          startPos = transform.position;
          GameController.instance.AddResetListener(Reset);
     }

     private void Reset() { 
          this.transform.position = startPos;
     }

     public static StaticObjectProps getProps(GameObject gameObject) { 
          StaticObjectProps props = gameObject.GetComponent<StaticObjectProps>();
          if(props == null) { 
              return gameObject.AddComponent<StaticObjectProps>();
          }
          return props;
     }
}