using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameLoopStub))]
public class GameLoopStubEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameLoopStub gameLoopStubScript = (GameLoopStub)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Start Game"))
        {
            gameLoopStubScript.StartGame();
        }

        if (GUILayout.Button("Stop On Death"))
        {
            gameLoopStubScript.StopOnDeath();
        }

        if (GUILayout.Button("Next Run"))
        {
            gameLoopStubScript.NextRun();
        }
    }
}
