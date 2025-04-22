public class StaticDock : BasicDock
{
    // Start is called before the first frame update
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        if (this.transform.GetChild(0).childCount > 0)
            _isEmpty = false;
        else
            _isEmpty = true;
    }
}
