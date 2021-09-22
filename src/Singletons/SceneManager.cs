using System.Collections.Generic;

using Godot;

namespace TowerDefenseMC.Singletons
{
    public class SceneManager : Node
    {
        public Node CurrentScene { get; set; }


        public override void _Ready()
        {
            Viewport root = GetTree().Root;
            CurrentScene = root.GetChild(root.GetChildCount() - 1);
        }

        /// <summary>
        /// Change current scene with the scene from the given path.
        /// </summary>
        /// <param name="path">Scene path</param>
        public void GotoScene(string path)
        {
            // CallDeferred to prevent unexpected behaviours:
            // This function could be called from a callback or 
            // some other function from the current scene.
            CallDeferred(nameof(DeferredGotoScene), path);
        }

        /// <summary>
        /// Change current (deferred) scene with the scene from the given path.
        /// </summary>
        /// <param name="path">Scene path</param>
        private void DeferredGotoScene(string path)
        {
            // Remove the current scene
            CurrentScene.Free();

            // Load a new scene.
            PackedScene nextScene = (PackedScene) GD.Load(path);
            CurrentScene = nextScene.Instance();
            GetTree().Root.AddChild(CurrentScene);
            
            GetTree().CurrentScene = CurrentScene;
        }

        /// <summary>
        /// Add a collection of nodes to the current scene.
        /// </summary>
        /// <param name="nodes">Collection of nodes to add</param>
        public void AddNodesToScene(IEnumerable<Node> nodes)
        {
            foreach(Node node in nodes)
            {
                AddNodeToScene(node);
            }
        }

        /// <summary>
        /// Add a single node to the current scene.
        /// </summary>
        /// <param name="node">Node to add</param>
        public void AddNodeToScene(Node node)
        {
            CurrentScene.AddChild(node);
        }

        /// <summary>
        /// Load and instantiate a node from the node's scene path.
        /// </summary>
        /// <param name="path">Node path</param>
        /// <returns>New instance of a node</returns>
        public Node LoadNode(string path)
        {
            PackedScene newNodeScene = (PackedScene) ResourceLoader.Load(path);
            return newNodeScene.Instance();
        }

        /// <summary>
        /// Change the parent of the given node with the newParent
        /// </summary>
        /// <param name="node">node to move</param>
        /// <param name="newParent">new node parent</param>
        public void ChangeParentNode(Node node, Node newParent)
        {
            if (node == null || newParent == null) return;
            
            Node oldParent = node.GetParent();

            oldParent?.RemoveChild(node);

            newParent.AddChild(node);
        }
    }
}