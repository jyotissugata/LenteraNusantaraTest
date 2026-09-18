import json
import uuid
import os

def new_id():
    return str(uuid.uuid4())

input_actions = {
    "name": "PlayerInputActions",
    "maps": [
        {
            "name": "Player",
            "id": new_id(),
            "actions": [
                { "name": "Move", "type": "Value", "id": new_id(), "expectedControlType": "Vector2", "processors": "", "interactions": "", "initialStateCheck": True },
                { "name": "Look", "type": "Value", "id": new_id(), "expectedControlType": "Vector2", "processors": "", "interactions": "", "initialStateCheck": True },
                { "name": "Interact", "type": "Button", "id": new_id(), "expectedControlType": "Button", "processors": "", "interactions": "", "initialStateCheck": False },
                { "name": "Sprint", "type": "Button", "id": new_id(), "expectedControlType": "Button", "processors": "", "interactions": "", "initialStateCheck": False }
            ],
            "bindings": [
                { "name": "WASD", "id": new_id(), "path": "2DVector", "interactions": "", "processors": "", "groups": "", "action": "Move", "isComposite": True, "isPartOfComposite": False },
                { "name": "up", "id": new_id(), "path": "<Keyboard>/w", "interactions": "", "processors": "", "groups": "", "action": "Move", "isComposite": False, "isPartOfComposite": True },
                { "name": "down", "id": new_id(), "path": "<Keyboard>/s", "interactions": "", "processors": "", "groups": "", "action": "Move", "isComposite": False, "isPartOfComposite": True },
                { "name": "left", "id": new_id(), "path": "<Keyboard>/a", "interactions": "", "processors": "", "groups": "", "action": "Move", "isComposite": False, "isPartOfComposite": True },
                { "name": "right", "id": new_id(), "path": "<Keyboard>/d", "interactions": "", "processors": "", "groups": "", "action": "Move", "isComposite": False, "isPartOfComposite": True },
                { "name": "", "id": new_id(), "path": "<Gamepad>/leftStick", "interactions": "", "processors": "", "groups": "", "action": "Move", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Pointer>/delta", "interactions": "", "processors": "", "groups": "", "action": "Look", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Gamepad>/rightStick", "interactions": "", "processors": "", "groups": "", "action": "Look", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Keyboard>/e", "interactions": "", "processors": "", "groups": "", "action": "Interact", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Gamepad>/buttonSouth", "interactions": "", "processors": "", "groups": "", "action": "Interact", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Keyboard>/leftShift", "interactions": "", "processors": "", "groups": "", "action": "Sprint", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Gamepad>/leftStickPress", "interactions": "", "processors": "", "groups": "", "action": "Sprint", "isComposite": False, "isPartOfComposite": False }
            ]
        },
        {
            "name": "UI",
            "id": new_id(),
            "actions": [
                { "name": "Cancel", "type": "Button", "id": new_id(), "expectedControlType": "Button", "processors": "", "interactions": "", "initialStateCheck": False },
                { "name": "Click", "type": "PassThrough", "id": new_id(), "expectedControlType": "Button", "processors": "", "interactions": "", "initialStateCheck": False },
                { "name": "Submit", "type": "Button", "id": new_id(), "expectedControlType": "Button", "processors": "", "interactions": "", "initialStateCheck": False },
                { "name": "Point", "type": "PassThrough", "id": new_id(), "expectedControlType": "Vector2", "processors": "", "interactions": "", "initialStateCheck": False }
            ],
            "bindings": [
                { "name": "", "id": new_id(), "path": "<Keyboard>/escape", "interactions": "", "processors": "", "groups": "", "action": "Cancel", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Gamepad>/buttonEast", "interactions": "", "processors": "", "groups": "", "action": "Cancel", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Mouse>/leftButton", "interactions": "", "processors": "", "groups": "", "action": "Click", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Keyboard>/enter", "interactions": "", "processors": "", "groups": "", "action": "Submit", "isComposite": False, "isPartOfComposite": False },
                { "name": "", "id": new_id(), "path": "<Mouse>/position", "interactions": "", "processors": "", "groups": "", "action": "Point", "isComposite": False, "isPartOfComposite": False }
            ]
        }
    ],
    "controlSchemes": []
}

target_path = r"d:\workspace\LenteraNusantaraTest\Assets\JyotisSugata\_Core\Input\PlayerInputActions.inputactions"
os.makedirs(os.path.dirname(target_path), exist_ok=True)
with open(target_path, "w", encoding="utf-8") as f:
    json.dump(input_actions, f, indent=4)
print(f"Created {target_path}")
