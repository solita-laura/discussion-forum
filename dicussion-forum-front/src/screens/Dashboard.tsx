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

  type newTopic = {
    topicname: string;
  }

  type Error = {
    errorMessage: string;
  }

  const [topics, setTopics] = useState<Topic[]>([]);
  const [newTopic, setNewTopic] = useState<newTopic>({
    topicname: '',
  });
  const [error, setError] = useState<Error>({
    errorMessage: '',
  });

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
          console.log(topics);
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

  const createTopic = async () => {
    await fetch('http://localhost:5055/api/Topics', {
      method: 'POST',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(newTopic),
    })
      .then(async response => {
        if (response.ok) {
          getTopics();
        } else {
          switch (response.status) {
            case 401:
              navigate('/login');
              break;
            default: 
              setError({ errorMessage: 'Error creating topic' });
              break; 
        }
      }});
    }

  useEffect(() => {
    getTopics();
  }, []);

  return (
    <div className="flex flex-col justify-center">
      <h1 className="top-0 w-full text-cyan-900 uppercase p-2">Topics</h1>
      <form className="w-full p-8">
        <label className="text-cyan-900">{error.errorMessage}</label>
        <input type="text"
          className="p-2 w-2/4 bg-gray-100 text-cyan-950 rounded"
          placeholder="Create a new topic"
          value={newTopic.topicname}
          onChange={(e) => setNewTopic({ ...newTopic, topicname: e.target.value })} />
      <button className="m-2"
        onClick={() => createTopic()}>
        Submit
      </button>
      </form>
      {topics.length>0 ? topics.map((topic) => (
        <div onClick={() => navigate('/topic', {state: {topicId: topic.id, topicname: topic.topicname}})} 
          className="p-3 hover:cursor-pointer" 
          key={topic.id}>
          <Topic
            topicname={topic.topicname}
            messagecount={topic.messagecount}
            lastupdated={new Date(topic.lastupdated)}/>
        </div>
      )): <h1>No topics found</h1>}
    </div>
  );
}

export default Dashboard;