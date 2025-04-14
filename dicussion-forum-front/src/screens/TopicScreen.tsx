import { useEffect, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import TopicMessage from "../components/TopicMessage";


function TopicScreen() {

    const navigate = useNavigate();
    const location = useLocation();

    const { topicId, topicname } = location.state;

    type Message = {
      id: number;
      content: string;
      upvotes: number
    }

    const [messages, setMessages] = useState<Message[]>([]);

    const getMessages = async () => {
      await fetch('http://localhost:5055/api/Topics/Message?topicid=' + topicId, {
        method: 'GET',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
      })
        .then(async response => {
          if (response.ok) {
            setMessages(await response.json());
          } else {
            switch (response.status) {
              case 401:
                navigate('/login');
                break;
              default: 
                setMessages([]);
                break; 
            }
          }
      });
    };

    useEffect(() => {
      getMessages();
    }, []);

  return (
    <div className="flex flex-wrap justify-center">
      <h1 className="fixed top-0 uppercase text-shadow-cyan-900">{topicname}</h1>
      <div></div>
      {messages ? messages.map((message) => (
        <div className="p-3" key={message.id}>
          <div className="border-1 border-cyan-950 rounded-sm p-2 w-2xl text-xs shadow-2xs h-auto">
            <TopicMessage content={message.content} upvotes={message.upvotes}/>
          </div>
        </div>
      )) : <h1>No messages found</h1>}
    </div>
  )
}

export default TopicScreen;