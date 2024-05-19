import React, { useState } from 'react';
import { AppBar, Toolbar, IconButton, Typography, Drawer, List, ListItem, ListItemText, CssBaseline, Box } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import { Link } from 'react-router-dom';

const drawerWidth = 240;

const Layout: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [drawerOpen, setDrawerOpen] = useState(false);

  const handleDrawerToggle = () => {
    setDrawerOpen(!drawerOpen);
  };

  return (
    <Box sx={{ display: 'flex' }}>
      <CssBaseline />
      <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
        <Toolbar>
          <IconButton edge="start" color="inherit" aria-label="menu" onClick={handleDrawerToggle} sx={{ mr: 2 }}>
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" noWrap>
            MyClass
          </Typography>
        </Toolbar>
      </AppBar>
      <Drawer
        variant="persistent"
        anchor="left"
        open={drawerOpen}
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
      >
        <Toolbar />
        <Box sx={{ overflow: 'auto' }}>
          <List>
            <ListItem button component={Link} to="/">
              <ListItemText primary="Home" />
            </ListItem>
            {/* Add more menu items as needed */}
          </List>
        </Box>
      </Drawer>
      <Box
        component="main"
        sx={{ flexGrow: 1, p: 3, transition: (theme) => theme.transitions.create('margin', {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.leavingScreen,
        }), marginLeft: drawerOpen ? `${drawerWidth}px` : 0 }}
      >
        <Toolbar />
        {children}
      </Box>
    </Box>
  );
};

export default Layout;
