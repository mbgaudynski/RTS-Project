using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private List<Agent> _selectedAgents; // List of currently selected agents

    // Start is called before the first frame update
    void Start()
    {
        // Selected agents list
        _selectedAgents = new List<Agent>();
    }

    // Update is called once per frame
    void Update()
    {
        // Detect LMB presses
        if (Input.GetMouseButtonDown(0))
        {
            string print = "";
            print += "LMB pressed. ";

            // Raycast from mouse position to detect object clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Change UI behavior based on what was hit
                string hitTag = hit.collider.gameObject.tag;
                // Clear currently selected agents if LMB press on ground
                if (hitTag == "Ground")
                {
                    print += "Clicked ground. Clearing selected. ";
                    ClearSelectedAgents();
                }
                // Select or add to selected (depending on shift key)
                else
                {
                    print += "Ground not clicked. ";

                    // Check for collider on hit object
                    GameObject go = hit.collider.gameObject;
                    if (go.TryGetComponent(out Agent agent))
                    {
                        print += "Clicked object has collider. ";

                        // Add to selected, if shift key is pressed
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
                        {
                            print += "Shift is pressed. ";
                            AddToSelectedAgents(agent);
                        }
                        // Clear selected and select clicked agent, if shift is not pressed
                        else
                        {
                            print += "No shift is pressed. ";
                            SelectAgent(go.GetComponent<Agent>());
                        }
                    }
                }
            }

            // Debug.Log(print);
        }

        // Detect RMB presses 
        if (Input.GetMouseButtonDown(1))
        {
            string print = "";
            print += "RMB pressed. ";

            // Raycast from mouse position to detect object clicked
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Set destination or rally point if click is on ground. 
                GameObject go = hit.collider.gameObject;
                Vector3 hitPoint = hit.point;
                if (go.tag == "Ground")
                {
                    print += "Clicked ground. ";

                    // Enqueue all moving agent desinations, and set all nonmoving agent rally points. 
                    foreach (Agent a in _selectedAgents)
                    {
                        // Enqueue MA destinations
                        if (a.gameObject.TryGetComponent(out MovingAgent ma))
                        {
                            print += "MA in selected. ";

                            // Enqueue destination if shift is pressed, set destination if shift is not pressed.
                            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                            {
                                print += "Shift is pressed. ";
                                ma.EnqueueDestination(hitPoint);
                            }
                            else
                            {
                                print += "No shift is pressed. ";
                                ma.SetDestination(hitPoint);
                            }
                        }
                        // Set NMA rally points
                        if (a.gameObject.TryGetComponent(out NonmovingAgent nma))
                        {
                            if (a.gameObject.TryGetComponent(out ResourceCollector rc))
                            {
                                //rc.SetRallyPoint(hitPoint);
                            }
                        }
                    }
                }
                
            }

             Debug.Log(print);
        }
    }

    // Clears list of currently selected agents
    private void ClearSelectedAgents()
    {
        _selectedAgents.Clear();
        // Debug.Log("Clearing selected");
    }

    // Clears list of currently selected agents and selected the passed-in agent
    private void SelectAgent(Agent agent)
    {
        ClearSelectedAgents();

        _selectedAgents.Add(agent);
        LogSelected();
    }

    // Adds agent to list of selected, if not already contained. Otherwise, removes from list of selected.
    private void AddToSelectedAgents(Agent agent)
    {
        if (!_selectedAgents.Contains(agent))
            _selectedAgents.Add(agent);
        else
            _selectedAgents.Remove(agent);

        LogSelected();
    }

    // TODO remove
    private void LogSelected()
    {
        string print = "";

        foreach (Agent a in _selectedAgents)
        {
            print += a.gameObject.name;

            if (_selectedAgents.IndexOf(a) < _selectedAgents.Count - 1)
                print += ", ";
            else if (_selectedAgents.IndexOf(a) == _selectedAgents.Count - 1)
                print += ".";
        }

        Debug.Log(print);
    }
}
