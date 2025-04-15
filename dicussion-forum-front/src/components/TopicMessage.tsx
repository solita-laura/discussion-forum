import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';

type MessageProps = {
  content: string;
  upvotes: number;
  postdate: Date
};

function TopicMessage (props: MessageProps) {
  return (
    <div className="">
      <p className="border-b-1 p-5">{props.content}</p>
      <div className="inline-flex items-center space-x-5 p-2">
        <div className='inline-flex space-x-1 items-center'>
        <FavoriteBorderIcon />
        <p className="text-xs">{props.upvotes}</p>
        </div>
        <p className="text-xs">{props.postdate.toLocaleDateString()}</p>
      </div>
    </div>
  );
}
export default TopicMessage;