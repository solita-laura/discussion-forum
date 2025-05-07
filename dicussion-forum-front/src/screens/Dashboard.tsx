import { useEffect, useState } from "react";
import Topic from "../components/Topic";
import { useNavigate } from "react-router-dom";
import HeaderBar from "../components/HeaderBar";
import { LogOutUser } from "../functions/LogOutUser";
import { GetUserId } from "../functions/GetUserId";

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
            setError({ errorMessage: 'Error creating topic' });
            return; 
          } 
        }
      );
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

    //Check that the topic name is less than 20 characters and more than 0
    if (updateTopicName.updateTopicName.length > 20 || updateTopicName.updateTopicName.length < 1) {
      setError({ errorMessage: 'Topic name must be between 1 and 20 characters.' });
      return;
    }
    
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
          setError({ errorMessage: 'Error updating the topic name' });
          return;
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
          setError({ errorMessage: 'Error deleting topic' });
          return;
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
    try{
      async function fetchUserId() {
        const id = await GetUserId();
        if (id!=null) {
            getTopics();
        }else{
          navigate('/login')
        }
      }
        fetchUserId();
      }
      catch{
          return;
      }
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

  const logOut = async(event: React.MouseEvent) => {
    event.preventDefault();
    if(await LogOutUser()){
      navigate('/login');
    }
  }

  return (
    <div className="flex flex-col justify-center">
      <HeaderBar
      topicname=""
      logOut={logOut}
      />
      <form className="w-full p-6 mt-8" onSubmit={createTopic}>
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