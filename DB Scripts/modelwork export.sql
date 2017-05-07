SELECT FROM_UNIXTIME(topic_time) 'Data utworzenia', FROM_UNIXTIME(topic_last_post_time) 'Data ostatniego posta', CONCAT('http://modelwork.pl/viewtopic.php?f=', forum_id, '&t=', topic_id) 'URL', topic_title 'Tytu�', topic_first_poster_name 'Autor' 
FROM phpbb3_topics 
WHERE forum_id = 64 
AND FROM_UNIXTIME(topic_time) > '2016-09-26 14:43:57' 
ORDER BY topic_time DESC