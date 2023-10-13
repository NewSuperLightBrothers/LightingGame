public struct UserInfo
{
    public string id;
    public string name;

    public override string ToString()
    {
        return $"PlayerID: {id}, PlayerName: {name}";
    }
}
