import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Container, Typography, Switch, Collapse, FormControlLabel, Box, Paper, FormGroup, List, ListItem, ListItemText, Accordion, AccordionSummary, AccordionDetails, IconButton } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

interface CourseNotification {
  id: number;
  name: string;
  isEnabled: boolean;
}

interface UserProfile {
  username: string;
  email: string;
}

interface UserNotifications {
  isEnabled: boolean;
  newAssignmentEnabled: boolean;
  deadlineReminderEnabled: boolean;
  gradingAssignmentEnabled: boolean;
  assignmentSubmittedEnabled: boolean;
}

interface UserSettings {
  profile: UserProfile;
  notifications: UserNotifications;
  courses: CourseNotification[];
}

const SettingsPage: React.FC = () => {
  const [userSettings, setUserSettings] = useState<UserSettings | null>(null);

  useEffect(() => {
    // Fetch user settings from the API
    axios.get<UserSettings>('/api/settings')
      .then(response => {
        setUserSettings(response.data);
      })
      .catch(error => {
        console.error('Error fetching user settings:', error);
      });
  }, []);

  const handleNotificationsToggle = () => {
    if (!userSettings) return;

    // Update user notifications settings
    const updatedSettings = { ...userSettings, notifications: { ...userSettings.notifications, isEnabled: !userSettings.notifications.isEnabled } };

    axios.put('/api/settings/notifications', updatedSettings.notifications)
      .then(response => {
        setUserSettings(updatedSettings);
      })
      .catch(error => {
        console.error('Error updating user notifications settings:', error);
      });
  };

  const handleCourseNotificationToggle = (courseId: number) => {
    if (!userSettings) return;

    // Find the course to update
    const updatedCourses = userSettings.courses.map(course =>
      course.id === courseId
        ? { ...course, isEnabled: !course.isEnabled }
        : course
    );

    // Update the user settings with the new courses
    const updatedSettings = { ...userSettings, courses: updatedCourses };

    setUserSettings(updatedSettings);

    // Update course notification settings
    axios.put(`/api/course/user-course/${courseId}/toggle-notifications`)
      .catch(error => {
        console.error('Error updating course notification settings:', error);
      });
  };

  const updateUserSettings = (updatedSettings: UserSettings) => {
    // Update UI immediately
    setUserSettings(updatedSettings);
  
    // Send PUT request to update user settings
    axios.put('/api/settings/notifications', updatedSettings.notifications)
      .then(response => {
        // Handle success if needed
      })
      .catch(error => {
        console.error('Error updating user settings:', error);
        // Handle error if needed
      });
  };
  

  const handleNewAssignmentToggle = () => {
    if (!userSettings) return;
    const updatedSettings = { ...userSettings, notifications: { ...userSettings.notifications, newAssignmentEnabled: !userSettings.notifications.newAssignmentEnabled } };
    updateUserSettings(updatedSettings);
  };
  
  const handleDeadlineReminderToggle = () => {
    if (!userSettings) return;
    const updatedSettings = { ...userSettings, notifications: { ...userSettings.notifications, deadlineReminderEnabled: !userSettings.notifications.deadlineReminderEnabled } };
    updateUserSettings(updatedSettings);
  };
  
  const handleGradingAssignmentToggle = () => {
    if (!userSettings) return;
    const updatedSettings = { ...userSettings, notifications: { ...userSettings.notifications, gradingAssignmentEnabled: !userSettings.notifications.gradingAssignmentEnabled } };
    updateUserSettings(updatedSettings);
  };
  
  const handleAssignmentSubmittedToggle = () => {
    if (!userSettings) return;
    const updatedSettings = { ...userSettings, notifications: { ...userSettings.notifications, assignmentSubmittedEnabled: !userSettings.notifications.assignmentSubmittedEnabled } };
    updateUserSettings(updatedSettings);
  };
  

  if (!userSettings) {
    return <Typography>Loading...</Typography>;
  }

  return (
    <Container sx={{ padding: '16px', mb: 3, display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
      <Paper elevation={3} sx={{ padding: '16px', mb: 3, width: '47.5rem' }}>
        <Typography variant="h4" gutterBottom>Profile</Typography>
        <List>
          <ListItem>
            <ListItemText primary="Username" secondary={userSettings.profile.username} />
          </ListItem>
          <ListItem>
            <ListItemText primary="Email" secondary={userSettings.profile.email} />
          </ListItem>
        </List>
      </Paper>

      <Paper elevation={3} sx={{ padding: '16px', width: '47.5rem' }}>
        <Typography variant="h4" gutterBottom>Notifications</Typography>
        <FormControlLabel
          control={<Switch checked={userSettings.notifications.isEnabled} onChange={handleNotificationsToggle} />}
          label="Enabled"
        />
        <Collapse in={userSettings.notifications.isEnabled}>
          <Box sx={{ padding: '16px' }}>
            <FormGroup>
            <FormControlLabel
  control={<Switch checked={userSettings.notifications.newAssignmentEnabled} onChange={() => handleNewAssignmentToggle()} />}
  label="New Assignment"
  labelPlacement="start"
  sx={{ justifyContent: 'space-between', marginLeft: 0 }}
/>
<FormControlLabel
  control={<Switch checked={userSettings.notifications.deadlineReminderEnabled} onChange={() => handleDeadlineReminderToggle()} />}
  label="Deadline Reminder"
  labelPlacement="start"
  sx={{ justifyContent: 'space-between', marginLeft: 0 }}
/>
<FormControlLabel
  control={<Switch checked={userSettings.notifications.gradingAssignmentEnabled} onChange={() => handleGradingAssignmentToggle()} />}
  label="Grading Assignment"
  labelPlacement="start"
  sx={{ justifyContent: 'space-between', marginLeft: 0 }}
/>
<FormControlLabel
  control={<Switch checked={userSettings.notifications.assignmentSubmittedEnabled} onChange={() => handleAssignmentSubmittedToggle()} />}
  label="Assignment Submitted"
  labelPlacement="start"
  sx={{ justifyContent: 'space-between', marginLeft: 0 }}
/>

            </FormGroup>
          </Box>
          <Accordion expanded={true} onChange={() => { }}>
            <AccordionSummary
              expandIcon={<ExpandMoreIcon />}
              aria-controls="panel1bh-content"
              id="panel1bh-header"
            >
              <Typography variant="subtitle1">Course Notifications</Typography>
            </AccordionSummary>
            <AccordionDetails>
              <List>
                {userSettings.courses.map(course => (
                  <ListItem key={course.id} sx={{ display: 'flex', justifyContent: 'space-between' }}>
                    <ListItemText primary={course.name} />
                    <Switch
                      checked={course.isEnabled}
                      onChange={() => handleCourseNotificationToggle(course.id)}
                    />
                  </ListItem>
                ))}
              </List>
            </AccordionDetails>
          </Accordion>
        </Collapse>
      </Paper>
    </Container>
  );
};

export default SettingsPage;
