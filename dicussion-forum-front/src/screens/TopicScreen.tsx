import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import TopicMessage from "../components/TopicMessage";
import MessageForm from "../components/MessageForm";


function TopicScreen() {

    const navigate = useNavigate();
    const location = useLocation();

    const { topicId, topicname } = location.state ?? {};

    type Message = {
      id: number;
      content: string;
      upvotes: number;
      postdate: Date;
    }

    type Error = {
      errorMessage: string;
    }

    type messageContent = {
      content: string;
    }

    const [messages, setMessages] = useState<Message[]>([]);
    const [error, setError] = useState<Error>({errorMessage: ''});
    const [messageContent, setMessageContent] = useState<messageContent>({content: ''});

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

    const addMessage = async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();

      if(!messageContent.content) {
        return;
      }
      
      await fetch('http://localhost:5055/api/Topics/Message', {
        method: 'POST',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          topicid: topicId,
          userid: 1, //for testing purposes
          content: messageContent.content,
        }),
      })
        .then(async response => {
          if (response.ok) {
            setMessageContent({content: ''});
            getMessages();
          } else {
              setError({ errorMessage: "Error adding message" });
            }
        }
      );
    };

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
      setMessageContent({
        ...messageContent,
        [event.target.name]: event.target.value
      });
    }

    useEffect(() => {
      getMessages();
    }, []);

  return (
    <div className="flex flex-col justify-center">
      <h1 className="fixed top-0 left-0 uppercase text-shadow-cyan-900 p-2 bg-white w-full">{topicname}</h1>
      <div className="mt-16">
      {messages ? messages.map((message) => (
        <div className="p-3" key={message.id}>
          <div className="border-1 border-cyan-950 rounded-sm p-2 w-2xl text-xs shadow-2xs h-auto">
            <TopicMessage 
              content={message.content}
              upvotes={message.upvotes}
              postdate = {new Date(message.postdate)}/>
          </div>
        </div>
      )) : <h1>No messages found</h1>}
      </div>
      <div>
          <MessageForm
            message={messageContent.content}
            addMessage={handleChange}
            sendMessage={addMessage}
            error = {error.errorMessage}
          />
      </div>
    </div>
  )
}

export default TopicScreen;