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

  type NewTopic = {
    topicname: string;
  }

  type TopicName = {
    topicname: string;
  }

  type Error = {
    errorMessage: string;
  }

  const [topics, setTopics] = useState<Topic[]>([]);
  const [newTopic, setNewTopic] = useState<NewTopic>({
    topicname: '',
  });
  const [error, setError] = useState<Error>({
    errorMessage: '',
  });
  const [topicName, setTopicName] = useState<TopicName>({
    topicname: '',
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

  const createTopic = async (event: React.FormEvent<HTMLFormElement>) => {

    event.preventDefault();

    if (!RegExp('^[a-zA-Z0-9 ]+$').test(newTopic.topicname)) {
      setError({ errorMessage: 'Topic name can only contain letters and numbers' });
      return;
    } 

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
          setError({ errorMessage: '' });
          setNewTopic({ topicname: '' });
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

  const sendTopic = async (event: React.FormEvent<HTMLFormElement>, topicid:number) => {
    event.preventDefault();

    if (!RegExp('^[a-zA-Z0-9 ]+$').test(topicName.topicname)) {
      setError({ errorMessage: 'Topic name can only contain letters and numbers' });
      return;
    } 

    await fetch('http://localhost:5055/api/Topics?topicid=' + topicid, {
      method: 'PUT',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(topicName.topicname),
    })
      .then(async response => {
        if (response.ok) {
          setError({ errorMessage: '' });
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

  const deleteTopic = async (event: React.MouseEvent, topicid:number) => {
    event.preventDefault();
    await fetch('http://localhost:5055/api/Topics?topicid=' + topicid, {
      method: 'DELETE',
      credentials: 'include',
      headers: {
        'Content-Type': 'application/json',
      },
    })
      .then(async response => {
        if (response.ok) {
          setError({ errorMessage: '' });
          getTopics();
        } else {
          switch (response.status) {
            case 401:
              navigate('/login');
              break;
            default: 
              setError({ errorMessage: 'Error deleting topic' });
              break; 
        }
      }});
  }



  useEffect(() => {
    getTopics();
  }, []);

  const addTopicName = async (event: React.ChangeEvent<HTMLInputElement>) => {
    setTopicName({
      ...topicName,
      [event.target.name]: event.target.value
    });
  }

  return (
    <div className="flex flex-col justify-center">
      <h1 className="top-0 w-full text-cyan-900 uppercase p-2">Topics</h1>
      <form className="w-full p-8" onSubmit={createTopic}>
        <label className="text-cyan-900">{error.errorMessage}</label>
        <input type="text"
          className="p-2 w-2/4 bg-gray-100 text-cyan-950 rounded"
          placeholder="Create a new topic"
          value={newTopic.topicname}
          onChange={(e) => setNewTopic({ ...newTopic, topicname: e.target.value })} />
      <button className="m-2">
        Submit
      </button>
      </form>
      {topics.length>0 ? topics.map((topic) => (
        <div onClick={() => navigate('/topic', {state: {topicId: topic.id, topicname: topic.topicname}})} 
          className="p-3 hover:cursor-pointer" 
          key={topic.id}>
          <Topic
            topicid={topic.id}
            topicname={topic.topicname}
            messagecount={topic.messagecount}
            lastupdated={new Date(topic.lastupdated)}
            addTopicName={addTopicName}
            sendTopicName={sendTopic}
            deleteTopic={deleteTopic}
            />
        </div>
      )): <h1>No topics found</h1>}
    </div>
  );
}

export default Dashboard;