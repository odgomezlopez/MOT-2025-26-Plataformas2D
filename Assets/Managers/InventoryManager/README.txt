=Requisitos=
- Input System https://docs.unity3d.com/Packages/com.unity.inputsystem@1.8/manual/Installation.html
-  https://assetstore.unity.com/packages/tools/utilities/serialized-dictionary-243052
-  https://gist.github.com/mstevenson/4325117

=Inventario=
1. Crea las acciones del Player Input
* En el action map de Player crea la acción "Inventory" para mostrar el inventario; y otra de "ContextualAction" para añadir un objecto al inventario.
* Crea un nuevo action map de Inventory y crea las acciones "Use" y "Exit"

2. Instancia los prefabs del InventoryManager e InventoryUI en la escena
3. Rellena las referencias a las acciones en GameObject de InventoryUI.

=Items=

==Definición==
1. El sistema de Items está basado en ScriptableObjects de la clase Items.
2. Los nuevos Items se crean creando un nuevo ScriptableObject de la clase Item (o de sus derivados).
3. Para crear nuevos Items, con otras acciones, se puede ver el ejemplo de Potion, se hereda de Item y se sobreescribe la función Use.

==Uso==
1. Instancia el prefab BaseItemContextualAction o BaseItemAutoAdd, dependiendo si se requiere una acción del jugador o no para añadirlo al inventario.
2. En el componente ItemController2D, modifica la propiedad ItemData para referenciar al Item que quieres utilizar.

=DLC=
* Si se quiere que la información se guarde en memoria, basta con cambiar la herencia de "InventoryManager", "ItemControler2D" e "PlayerContextualActionTriggeredV2" de MonoBehaviour a MonoBehaviourSaveable.