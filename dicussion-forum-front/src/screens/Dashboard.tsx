import { useEffect, useState } from "react";
import Topic from "../components/Topic";
import { useNavigate } from "react-router-dom";

/**
 * Dashboard present all the topics created to the discussion forum
 */


function Dashboard() {

  type Topic = {
    id: number;
    topicname: string;
    messagecount: number;
    lastupdated: Date;
    userid: string;
  }

  type NewTopic = {
    topicname: string;
  }

  type UpdateTopicName = {
    updateTopicName: string;
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
  const [updateTopicName, setUpdateTopicName] = useState<UpdateTopicName>({
    updateTopicName: '',
  });

  const navigate = useNavigate();

  /**
   * Fetches all the topics from the API
   */
  const getTopics = async () => {

    try{
      await fetch('http://localhost:5055/api/Topics', {
        method: 'GET',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
      })
      .then(async response => {
        if (response.ok) {
          setError({ errorMessage: '' });
          setTopics(await response.json());
        } else {
          setError({ errorMessage: 'Error fetching topics' });
          setTopics([]);
          navigate('/login');
          }
      });
    }catch{
      setError({ errorMessage: 'Error fetching topics' });
    }
  };

  /**
   * Create a new topic
   * @param event FormEvent
   */

  const createTopic = async (event: React.FormEvent<HTMLFormElement>) => {

    event.preventDefault();

    //Check that the topic name is less than 20 characters and more than 0
    if (newTopic.topicname.length > 20 || newTopic.topicname.length < 1) {
      setError({ errorMessage: 'Topic name must be between 1 and 20 characters.' });
      return;
    }

    // Check that the topic name follows the rules (only numbers and letters)
    if (!checkTopicName(newTopic.topicname)) {
      setError({ errorMessage: 'Topic name can only contain letters and numbers and white spaces.' });
      return;
    }

    try{
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
        }
      });
    }catch{
      setError({ errorMessage: 'Error creating topic' });
    }
  }

  /**
   * Update the topic name
   * @param event FormEvent
   * @param topicid number
   */

  const sendUpdatedName = async (event: React.FormEvent<HTMLFormElement>, topicid:number) => {
    event.preventDefault();

    if (!checkTopicName(updateTopicName.updateTopicName)) {
      setError({ errorMessage: 'Topic name can only contain letters, numbers and white spaces' });
      return;
    } 

    try{
      await fetch('http://localhost:5055/api/Topics?topicid=' + topicid, {
        method: 'PUT',
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(updateTopicName.updateTopicName),
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
              setError({ errorMessage: 'Error updating the topic name' });
              break; 
          }
      }
    });
    } catch {
      setError({errorMessage: 'Error updating topic name'});
    }
  }

  /**
   * Delete a topic
   * @param event MouseEvent
   * @param topicid number
   */

  const deleteTopic = async (event: React.MouseEvent, topicid:number) => {
    event.preventDefault();

    try{
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
        }
      });
    } catch {
      setError({errorMessage: 'Error deleting topic'})
    }
  }

  /**
   * check topic name 
   * @returns boolean
   */

  const checkTopicName = (nameToValidate: string): boolean => {
    return RegExp('^[a-zA-Z0-9 ]+$').test(nameToValidate);
  }

  /**
   * load topics
   */

  useEffect(() => {
    getTopics();
  }, []);

  /**
   * Add topic name
   * @param event ChangeEvent
   */

  const addTopicName = async (event: React.ChangeEvent<HTMLInputElement>) => {
    setUpdateTopicName({
      ...updateTopicName,
      [event.target.name]: event.target.value
    });
  }

  return (
    <div className="flex flex-col justify-center">
      <h1 className="top-0 w-full text-cyan-900 uppercase p-2">Topics</h1>
      <form className="w-full p-8" onSubmit={createTopic}>
        <label className="text-cyan-900 flex justify-center">{error.errorMessage}</label>
        <input type="text"
          className="p-2 w-fit bg-gray-100 text-cyan-950 rounded"
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
            userid={topic.userid}
            addTopicName={addTopicName}
            sendTopicName={sendUpdatedName}
            deleteTopic={deleteTopic}
            />
        </div>
      )): <h1>No topics found</h1>}
    </div>
  );
}

export default Dashboard;