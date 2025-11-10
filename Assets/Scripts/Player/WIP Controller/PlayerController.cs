//using UnityEngine;

//public class PlayerController : MonoBehaviour
//{
//    public PlayerInputHandler input { get; private set; }
//    public PlayerMovement movement { get; private set; }
//    public PlayerDash dash { get; private set; }
//    public PlayerAttack attack { get; private set; }
//    public PlayerHealth health { get; private set; }

//    void Awake()
//    {
//        input = GetComponent<PlayerInputHandler>();
//        movement = GetComponent<PlayerMovement>();
//        dash = GetComponent<PlayerDash>();
//        attack = GetComponent<PlayerAttack>();
//        health = GetComponent<PlayerHealth>();
//    }

//    void Update()
//    {
//        //coordination logic goes here (anything with constant input/output
//        movement.HandleMovement(input);
//        //dash.HandleDash(input);
//        //attack.HandleAttack(input);
//    } 
//}
