import { useEffect, useState } from "react";
import Topic from "../components/Topic";
import { useNavigate } from "react-router-dom";

function Dashboard() {

  type Topic = {
    id: number;
    topicname: string;
    messagecount: number;
    lastupdated: Date;
  }

  const [topics, setTopics] = useState<Topic[]>([]);

  const navigate = useNavigate();

  const getTopics = async () => {
    await fetch('http://localhost:5055/api/Topics', {
      method: 'GET',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    })
      .then(async response => {
        if (response.ok) {
          console.log(response);
          setTopics(await response.json());
        } else {
          switch (response.status) {
            case 401:
              navigate('/login');
              break;
            default: 
              setTopics([]);
              break; 
        }
      }});
  };

  useEffect(() => {
    getTopics();
  }, []);

  return (
    <div className="flex flex-wrap justify-center">
      <h1 className="fixed top-0 w-full text-cyan-900 uppercase">Topics</h1>
      {topics ? topics.map((topic) => (
        <div onClick={() => navigate('/topic', {state: {topicId: topic.id, topicname: topic.topicname}})} 
          className="p-3 hover:cursor-pointer" 
          key={topic.id}>
          <Topic
            topicname={topic.topicname}
            messagecount={topic.messagecount}
            lastupdated={new Date(topic.lastupdated)}/>
        </div>
      )) : <h1>No topics found</h1>}
    </div>
  );
}

export default Dashboard;