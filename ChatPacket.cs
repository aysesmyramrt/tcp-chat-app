public class ChatPacket
{
    public byte MessageType; // 1: Mesaj, 2: Baðlan, 3: Ayrýl, 4: ACK, vb.
    public int Length;
    public string Payload;
}