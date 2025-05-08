import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import { useNavigate } from 'react-router-dom';

type BarProps = {
    topicname: string;
    logOut: (event: React.MouseEvent) => void;
}

export default function HeaderBar(props: BarProps) {

  const navigation = useNavigate();

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar>
        <Toolbar className='bg-cyan-900 text-cyan-50 uppercase'>
        <Typography variant="h6" component="div" sx={{ mr: 2}} onClick={() => navigation('/dashboard')} className='cursor-pointer'>
            Discussion Forum
          </Typography>
          <Typography variant="h6" component="div" sx={{ mr: 2, flexGrow: 1 }}>
            {props.topicname}
          </Typography>
          <Button color="inherit" onClick={props.logOut}>Log out</Button>
        </Toolbar>
      </AppBar>
    </Box>
  );
}

