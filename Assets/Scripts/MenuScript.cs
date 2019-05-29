using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{

    public Color selectedColor = Color.red;
    public Color unselectedColor = Color.white;
    public Camera Camera;

    public GameObject selectedCharacter = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 100.0f))
        {
            if(hit.transform)
            {
                if(Input.GetMouseButtonDown(0))
                {
                    if(hit.transform.tag == "Player")
                    {
                        GetComponent<AudioSource>().Play();
                        if (selectedCharacter)
                        {
                            selectedCharacter.transform.Find("Base").GetComponent<Renderer>().material.color = unselectedColor;
                            selectedCharacter.transform.Find("Name").GetComponent<Renderer>().material.color = unselectedColor;
                        }
                        selectedCharacter = hit.transform.gameObject;
                        if (selectedCharacter.name == "Haj") {
                            GameObject.FindGameObjectWithTag("characterSelector").GetComponent<CharacterSelection>().Character = Character.Haj;
                        } else {
                            GameObject.FindGameObjectWithTag("characterSelector").GetComponent<CharacterSelection>().Character = Character.Khaliga;
                        }
                        hit.transform.Find("Base").GetComponent<Renderer>().material.color = selectedColor;
                        hit.transform.Find("Name").GetComponent<Renderer>().material.color = selectedColor;
                        StartCoroutine(Animate(hit.transform));
                    }
                }
            }
        }
    }

    private IEnumerator Animate(Transform target)
    {
        Transform lpGuy = target.Find("lp_guy");
        CharacterMenu characterMenu = lpGuy.GetComponent<CharacterMenu>();
        characterMenu.rotate = false;

        var _direction = (Camera.transform.position - lpGuy.position).normalized;
        _direction.y = 90;

        lpGuy.rotation = Quaternion.LookRotation(_direction);

        //lpGuy.rotation = Quaternion.Slerp(lpGuy.rotation, Quaternion.Euler(-90, 0, 180), 20 * Time.time);

        characterMenu.animator.SetBool("is_in_air", true);
        yield return new WaitForSecondsRealtime(0.5f);
        characterMenu.animator.SetBool("is_in_air", false);
        yield return new WaitForSecondsRealtime(0.5f);

        characterMenu.rotate = true;
    }

    public void Next()
    {
        print("Next");
    }

    public void Quit()
    {
        print("Quit");
    }
}
