using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ReLocation : MonoBehaviour
{
    public float baseBackGroundScale = 30;
    private Collider2D _collider;

    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DetectGround"))
        {
            Vector2 playerPosition = GameManager.Instance.player.transform.position;
            float dirX = playerPosition.x - transform.position.x;
            float dirY = playerPosition.y - transform.position.y;

            float distanceX = Math.Abs(dirX);
            float distanceY = Math.Abs(dirY);

            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;

            switch (transform.tag)
            {
                case ("BackGround"):
                    if(distanceX < distanceY)
                    {
                        transform.Translate(Vector2.up * (dirY * baseBackGroundScale * 2));
                    }
                    else if(distanceX > distanceY)
                    {
                        transform.Translate(Vector2.right * (dirX * baseBackGroundScale * 2));
                    }
                    else //대각선으로 정확히 움직였을 때 재배치
                    {
                        transform.Translate(Vector2.right * (dirX * baseBackGroundScale * 2));
                        transform.Translate(Vector2.up * (dirY * baseBackGroundScale * 2));
                    }
                    break;

                case("Enemy"):
                    if (_collider.enabled)
                    {
                        Vector2 randis = new Vector2(Random.Range(-2f, 2f), Random.Range(-4f, 4f));
                        if(distanceX < distanceY)
                        {
                            transform.Translate(Vector2.up * (dirY * baseBackGroundScale) + randis);
                        }
                        else if(distanceX > distanceY)
                        {
                            transform.Translate(Vector2.right * (dirX * baseBackGroundScale) + randis);
                        }
                        else //대각선으로 정확히 움직였을 때 재배치
                        {
                            transform.Translate(Vector2.right * (dirX * baseBackGroundScale) + randis);
                            transform.Translate(Vector2.up * (dirY * baseBackGroundScale) + randis);
                        }
                    }
                    break;
            }
        }
    }
}
